using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GarageSpaceAPI.Contracts;
using GarageSpace.Data.Models.EF;
using GarageSpace.Repository.Interfaces.EF;
using GarageSpace.Services.Interfaces;
using GarageSpaceAPI.Contracts.Dto;
using GarageSpace.Contracts;

namespace GarageSpace.Services;

public class UsersService : IUsersService
{
    private readonly IEFUserRepository _userRepository;
    private readonly AppSettings _appSettings;
    private readonly IPasswordHasher<Models.User> _passwordHashService;
    private readonly ILogger<UsersService> _logger;
    private readonly IMapper _mapper;
    private readonly IEventsPublisher _publisher;

    public UsersService(
        IEFUserRepository userRepository,
        IOptions<AppSettings> appSettings,
        IPasswordHasher<Models.User> passwordHashService,
        ILogger<UsersService> logger,
        IMapper mapper,
        IEventsPublisher publisher
        )
    {
        _userRepository = userRepository;
        _passwordHashService = passwordHashService;
        _appSettings = appSettings.Value;
        _logger = logger;
        _mapper = mapper;
        _publisher = publisher;
    }

    public async Task<string?> GetToken(string email, string password)
    {
        var userEntity = await _userRepository.GetByEmailAsync(email);

        var user = new Models.User
        {
            Id = userEntity.Id,
            Email = userEntity.Email,
            Nickname = userEntity.Nickname,
            Password = userEntity.Password
        };

        var passwordVerificationResult = _passwordHashService.VerifyHashedPassword(user, user.Password, password);
        if (passwordVerificationResult != PasswordVerificationResult.Success)
        {
            throw new ApplicationException("Password Hash Verification failed");
        }

        var key = Encoding.ASCII.GetBytes(_appSettings.JWTSecretKey);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.UserData, user.Id.ToString()),
                }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<long> Register(UserDto userRequest)
    {
        var userExistsEmailCheck = await CheckUserExists("email", userRequest.Email);
        if (userExistsEmailCheck)
        {
            throw new ApplicationException("User already exists");
        }
        
        var userExistsNicknameCheck = await CheckUserExists("nickname", userRequest.Nickname);
        if (userExistsNicknameCheck)
        {
            throw new ApplicationException("User already exists");
        }

        var user = new Models.User
        {
            Email = userRequest.Email,
            Nickname = userRequest.Nickname,
        };
        
        var hashedPassword = _passwordHashService.HashPassword(user, userRequest.Password);
        user.Password = hashedPassword;
        
        var hashedUser = _mapper.Map<User>(user);
        try
        {
            var createdUser = await _userRepository.CreateAsync(hashedUser);
            return createdUser.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError("User '{0}' creation failed with error {1}", user.Email, ex.Message);
            throw;
        }
    }

    public async Task<bool> CheckUserExists(string property, string value)
    {
        switch (property)
        {
            case "email":
            {
                var user = await _userRepository.GetByEmailAsync(value);
                return user != null;
            }
            //case "nickname":
            //{
            //    var user = await _userRepository.Get (value);
            //    return user != null;
            //}
            default:
                return false;
        }
    }

    public async Task<UserDto> GetUserById(long id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return _mapper.Map<UserDto>(user);
    }

    public Task<UserDto> GetByNickname(string name)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateUser(long id, UserDto user)
    {
        throw new NotImplementedException();
    }

    public async Task<long> CreateUser(UserDto dto)
    {
        try
        {
            var user = _mapper.Map<User>(dto);

            var createdEntity = await _userRepository.CreateAsync(user);
            
            PublishCreateUserEvent(createdEntity.Id, dto.Name);

            return createdEntity.Id;
        }
        catch (Exception ex) 
        {
            throw;
        }
    }

    private void PublishCreateUserEvent(long userId, string name)
    {
        var msg = new UserCreated 
        {
            UserId = userId,
            Name = name
        };


        //_publisher.Publish(msg);
    }
}