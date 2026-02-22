using Microsoft.EntityFrameworkCore;
using GarageSpace.Repository.Interfaces.EF;
using User = GarageSpace.Models.Repository.EF.User;

namespace GarageSpace.Repository.EntityFramework;

public class UserRepository : BaseRepository<User>, IEFUserRepository
{
    private readonly MainDbContext _context;

    public UserRepository(MainDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IList<User>> SearchAsync(int take, int skip)
    {
        return await _context.Users
            .Skip(skip)
            .Take(take)
            .Include(u => u.Garage)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Users.CountAsync();
    }

    public async Task<IList<User>> ListAllAsync()
    {
        return await _context.Users
            .Include(u => u.Garage)
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        return await _context.Users
            .Include(u => u.Garage)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));

        return await _context.Users
            .Include(u => u.Garage)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> CreateAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ArgumentException("Email cannot be null or empty.", nameof(user));

        // Check if user with same email already exists
        var existingUser = await GetByEmailAsync(user.Email);
        if (existingUser != null)
            throw new InvalidOperationException($"User with email {user.Email} already exists.");

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UpdateAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ArgumentException("Email cannot be null or empty.", nameof(user));

        var existingUser = await _context.Users.FindAsync(user.Id);
        if (existingUser == null)
            return false;

        // Check if email is being changed and if the new email is already taken
        if (existingUser.Email != user.Email)
        {
            var userWithNewEmail = await GetByEmailAsync(user.Email);
            if (userWithNewEmail != null)
                throw new InvalidOperationException($"User with email {user.Email} already exists.");
        }

        _context.Entry(existingUser).CurrentValues.SetValues(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
