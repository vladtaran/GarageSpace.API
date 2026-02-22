namespace GarageSpace.API.Contracts.Request;

public class UserCreateRequest
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Nickname { get; set; }
    public string Password { get; set; }
}