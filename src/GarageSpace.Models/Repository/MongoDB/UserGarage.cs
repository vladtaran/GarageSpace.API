namespace GarageSpace.Models.Repository.MongoDB;

public class UserGarage
{
    public User? UserData { get; set; }
    
    public IEnumerable<Car>? Cars { get; set; }

}