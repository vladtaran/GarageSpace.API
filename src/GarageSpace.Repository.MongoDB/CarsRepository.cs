using MongoDB.Bson;
using GarageSpace.Repository.MongoDB.DbContext;
using MongoDB.Driver;
using GarageSpace.Repository.Interfaces.MongoDB;
using GarageSpace.Models.Repository.MongoDB;

namespace GarageSpace.Repository.MongoDB;

public class CarsRepository : IMongoDbCarsRepository
{
    private readonly IMongoDbContext _dbContext;
    
    public CarsRepository(IMongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<Car>> Search(int take, int skip)
    {
        var result =  await _dbContext.Cars.Find(new BsonDocument()).ToListAsync();
        
        return result
            .Take(take)
            .Skip(skip)
            .ToList();
    }

    public async Task<IList<Car>> ListAll()
    {
        return await _dbContext.Cars.Find(c => true).ToListAsync();
    }

    public async Task<Car> GetById(long id)
    {
        return await _dbContext.Cars.Find(c => c.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IList<Car>> GetByUserId(long userId)
    {
        return await _dbContext.Cars.Find(c => c.CreatedBy.Id == userId).ToListAsync();
    }

    public async Task<long> Create(Car car)
    {
        await _dbContext.Cars.InsertOneAsync(car);
        return car.Id;
    }

    public async Task Update(long id, Car car)
    {
        await _dbContext.Cars.ReplaceOneAsync(c => c.Id == id, car);
    }

    public async Task Delete(long id)
    {
        await _dbContext.Cars.DeleteOneAsync(c => c.Id == id);
    }
}