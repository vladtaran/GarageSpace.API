using MongoDB.Driver;
using GarageSpace.Models.Repository.MongoDB;

namespace GarageSpace.Repository.MongoDB.DbContext;

public interface IMongoDbContext
{
    IMongoCollection<User> Users { get; }
    IMongoCollection<Car> Cars { get; }
}