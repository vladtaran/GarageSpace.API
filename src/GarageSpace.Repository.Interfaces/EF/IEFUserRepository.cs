using GarageSpace.Models.Repository.EF;

namespace GarageSpace.Repository.Interfaces.EF;

/// <summary>
/// Repository interface for managing User entities in the database.
/// </summary>
public interface IEFUserRepository
{
    /// <summary>
    /// Searches for users with pagination.
    /// </summary>
    /// <param name="take">Number of records to take.</param>
    /// <param name="skip">Number of records to skip.</param>
    /// <returns>A list of users matching the search criteria.</returns>
    Task<IList<User>> SearchAsync(int take, int skip);

    /// <summary>
    /// Gets the total count of users in the database.
    /// </summary>
    /// <returns>The total number of users.</returns>
    Task<int> GetTotalCountAsync();

    /// <summary>
    /// Retrieves all users from the database.
    /// </summary>
    /// <returns>A list of all users.</returns>
    Task<IList<User>> ListAllAsync();

    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    Task<User?> GetByIdAsync(long id);

    /// <summary>
    /// Retrieves a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Creates a new user in the database.
    /// </summary>
    /// <param name="user">The user entity to create.</param>
    /// <returns>The created user with updated properties.</returns>
    Task<User> CreateAsync(User user);

    /// <summary>
    /// Updates an existing user in the database.
    /// </summary>
    /// <param name="user">The user entity to update.</param>
    /// <returns>True if the update was successful; otherwise, false.</returns>
    Task<bool> UpdateAsync(User user);

    /// <summary>
    /// Deletes a user from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete.</param>
    /// <returns>True if the deletion was successful; otherwise, false.</returns>
    Task<bool> DeleteAsync(long id);
} 