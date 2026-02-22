using GarageSpace.Data.Models;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GarageSpace.Models.Repository.MongoDB;

[BsonIgnoreExtraElements]
public class User
{
    public User()
    {
        Address = new AddressInfo();
    }

    [Required]
    public long Id { get; set; }
    
    public string? Name { get; set; }
    
    public string? Surname { get; set; }
    
    public string? Nickname { get; set; }
    
    public string? Email { get; set; }
    
    public string? Password { get; set; }
    
    public string? Phone { get; set; }
    
    public string? Description { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public Guid PhotoId { get; set; }

    public int DriverExperience { get; set; }

    public GenderEnum Gender { get; set; }
    
    public AddressInfo Address { get; set; }
}
