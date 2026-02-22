using GarageSpace.Models.Repository.MongoDB;

namespace GarageSpace.Repository.Interfaces.MongoDB;

public interface IMongoDbUserRepository
{
    Task<List<User>> ListAll();
    Task<User> GetById(long id);
    Task<User> GetByEmail(string email);
    Task<User> GetByNickname(string name);
    Task<long> Create(User user);
    Task<bool> Update(long id, User user);
    Task Delete(long id);
}