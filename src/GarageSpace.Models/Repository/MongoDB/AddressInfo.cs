using System.ComponentModel.DataAnnotations.Schema;
using GarageSpace.Models.Repository.EF;

namespace GarageSpace.Models.Repository.MongoDB;

public class AddressInfo
{
    public Country? Country { get; set; }
    public string? City { get; set; }
}
