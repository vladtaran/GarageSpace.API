namespace GarageSpaceAPI.Contracts.Request;

public class CheckUserExistRequest
{
    public string? Property { get; set; }
    public string? Value { get; set; }
}