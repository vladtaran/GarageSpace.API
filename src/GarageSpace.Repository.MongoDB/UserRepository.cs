using MongoDB.Bson;
using MongoDB.Driver;
using GarageSpace.Repository.MongoDB.DbContext;
using GarageSpace.Repository.Interfaces.MongoDB;
using GarageSpace.Models.Repository.MongoDB;

namespace GarageSpace.Repository.MongoDB;

public class UserRepository : IMongoDbUserRepository
{
    private readonly IMongoDbContext _dbContext;

    public UserRepository(IMongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<User>> ListAll()
    {
        return await _dbContext.Users.Find(new BsonDocument()).ToListAsync();
    }

    public async Task<User> GetById(long id)
    {
        return await _dbContext.Users.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<User> GetByEmail(string email)
    {
        return await _dbContext.Users.Find(x => x.Email == email).FirstOrDefaultAsync();
    }

    public async Task<User> GetByNickname(string name)
    {
        return await _dbContext.Users.Find(x => x.Nickname == name).FirstOrDefaultAsync();
    }

    public async Task<long> Create(User user)
    {
        await _dbContext.Users.InsertOneAsync(user);
        return user.Id;
    }

    public async Task<bool> Update(long id, User user)
    {
        var result = await _dbContext.Users.UpdateOneAsync(
            Builders<User>.Filter.Eq(x => x.Id, id), 
            Builders<User>.Update
                .Set(x => x.Name, user.Name)
                .Set(x => x.Description, user.Description)
                .Set(x => x.Surname, user.Surname)
                .Set(x => x.PhotoId, user.PhotoId)
                .Set(x => x.Phone, user.Phone)
                .Set(x => x.Gender, user.Gender)
                .Set(x => x.DriverExperience, user.DriverExperience)
                .Set(x => x.DateOfBirth, user.DateOfBirth)
                .Set(x => x.Address, user.Address)
            );

        return result.ModifiedCount == 1;
    }

    public async Task Delete(long id)
    {
        await _dbContext.Users.DeleteOneAsync(x => x.Id == id);
    }
}