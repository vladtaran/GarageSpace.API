using Car = GarageSpace.Models.Repository.EF.Vehicles.Car;

namespace GarageSpace.Repository.Interfaces.EF;

/// <summary>
/// Repository interface for managing Car entities in the database.
/// </summary>
public interface IEFCarsRepository
{
    /// <summary>
    /// Searches for cars with pagination.
    /// </summary>
    /// <param name="take">Number of records to take.</param>
    /// <param name="skip">Number of records to skip.</param>
    /// <returns>A list of cars matching the search criteria.</returns>
    Task<IList<Car>> SearchAsync(int take, int skip);

    /// <summary>
    /// Gets the total count of cars in the database.
    /// </summary>
    /// <returns>The total number of cars.</returns>
    Task<int> GetTotalCountAsync();

    /// <summary>
    /// Retrieves all cars from the database.
    /// </summary>
    /// <returns>A list of all cars.</returns>
    Task<IList<Car>> ListAllAsync();

    /// <summary>
    /// Retrieves a car by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the car.</param>
    /// <returns>The car if found; otherwise, null.</returns>
    Task<Car?> GetByIdAsync(long id);

    /// <summary>
    /// Creates a new car in the database.
    /// </summary>
    /// <param name="car">The car entity to create.</param>
    /// <returns>The created car with updated properties.</returns>
    Task<Car> CreateAsync(Car car);

    /// <summary>
    /// Updates an existing car in the database.
    /// </summary>
    /// <param name="car">The car entity to update.</param>
    /// <returns>True if the update was successful; otherwise, false.</returns>
    Task<bool> UpdateAsync(Car car);

    /// <summary>
    /// Deletes a car from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the car to delete.</param>
    /// <returns>True if the deletion was successful; otherwise, false.</returns>
    Task<bool> DeleteAsync(long id);
}