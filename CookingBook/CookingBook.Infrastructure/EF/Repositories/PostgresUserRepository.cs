using CookingBook.Domain;
using CookingBook.Domain.Entities;
using CookingBook.Domain.ValueObjects;
using CookingBook.Infrastructure.EF.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CookingBook.Infrastructure.EF.Repositories;

public class PostgresUserRepository : IUserRepository
{
    private readonly DbSet<User> _users;
    private readonly WriteDbContext _writeDbContext;

    public PostgresUserRepository(WriteDbContext writeDbContext)
    {
        _users = writeDbContext.Users;
        _writeDbContext = writeDbContext;
    }

    public async Task<User> GetAsync(UserId id)
        => await _users
            .Include("_recipes")
            .FirstOrDefaultAsync(u => u.Id == id);

    public async Task AddAsync(User user)
    {
        await _users.AddAsync(user);
        await _writeDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _users.Update(user);
        await _writeDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _users.Remove(user);
        await _writeDbContext.SaveChangesAsync();
    }
}