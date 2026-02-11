using GarageSpaceAPI.Contracts.Dto;
using GarageSpaceAPI.Contracts.Request;

namespace GarageSpace.API.Mapping
{
    public class UserObjectsMapper : IUserObjectsMapper
    {
        public UserDto MapCreateUserRequestToUser(UserCreateRequest request) 
        {
            return new UserDto
            {
                Email = request.Email,
                Name = request.Name,
                Nickname = request.Nickname,
                Password = request.Password
            };
        }
    }
}
