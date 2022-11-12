using CookingBook.Application.Services;
using CookingBook.Infrastructure.EF.Contexts;
using CookingBook.Infrastructure.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace CookingBook.Infrastructure.EF.Services;

public class PostgresUserReadService : IUserReadService
{
    private readonly DbSet<UserReadModel> _users;

    public PostgresUserReadService(ReadDbContext readDbContext)
    {
        _users = readDbContext.Users;
    }

    public async Task<bool> ExistsByUserName(string userName)
        => await _users.AnyAsync(u => u.UserName.Equals(userName));
}