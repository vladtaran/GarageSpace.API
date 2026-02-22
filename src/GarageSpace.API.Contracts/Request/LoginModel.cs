namespace GarageSpace.API.Contracts.Request;

public class LoginModel : IUserCredentials
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}