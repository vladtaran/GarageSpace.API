using GarageSpace.Data.Models.EF;

namespace GarageSpace.Repository.Interfaces.EF;

/// <summary>
/// Repository interface for managing UserGarage entities in the database.
/// </summary>
public interface IEFGaragesRepository : IBaseRepository
{
    /// <summary>
    /// Searches for user garages with pagination.
    /// </summary>
    /// <param name="take">Number of records to take.</param>
    /// <param name="skip">Number of records to skip.</param>
    /// <returns>A list of user garages matching the search criteria.</returns>
    Task<IList<UserGarage>> SearchAsync(int take, int skip);

    /// <summary>
    /// Gets the total count of user garages in the database.
    /// </summary>
    /// <returns>The total number of user garages.</returns>
    Task<int> GetTotalCountAsync();

    /// <summary>
    /// Retrieves all user garages from the database.
    /// </summary>
    /// <returns>A list of all user garages.</returns>
    Task<IList<UserGarage>> ListAllAsync();

    /// <summary>
    /// Retrieves a user garage by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user garage.</param>
    /// <returns>The user garage if found; otherwise, null.</returns>
    Task<UserGarage?> GetByIdAsync(long id);

    /// <summary>
    /// Retrieves a user garage by user ID.
    /// </summary>
    /// <param name="ownerId">The ID of the user.</param>
    /// <returns>The user garage if found; otherwise, null.</returns>
    Task<UserGarage?> GetByOwnerIdAsync(long ownerId);

    /// <summary>
    /// Creates a new user garage in the database.
    /// </summary>
    /// <param name="userGarage">The user garage entity to create.</param>
    /// <returns>The created user garage with updated properties.</returns>
    Task<UserGarage> CreateAsync(UserGarage userGarage);

    /// <summary>
    /// Updates an existing user garage in the database.
    /// </summary>
    /// <param name="userGarage">The user garage entity to update.</param>
    /// <returns>True if the update was successful; otherwise, false.</returns>
    Task<bool> UpdateAsync(UserGarage userGarage);

    /// <summary>
    /// Deletes a user garage from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the user garage to delete.</param>
    /// <returns>True if the deletion was successful; otherwise, false.</returns>
    Task<bool> DeleteAsync(long id);
}