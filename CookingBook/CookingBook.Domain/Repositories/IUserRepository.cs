using CookingBook.Domain.Entities;
using CookingBook.Domain.ValueObjects;

namespace CookingBook.Domain;

public interface IUserRepository
{
    Task<User> GetAsync(UserId id);
    
    Task AddAsync(User user);
    
    Task UpdateAsync(User user);
    
    Task DeleteAsync(User user);
}