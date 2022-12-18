using CookingBook.Domain.Entities;
using CookingBook.Domain.ValueObjects;

namespace CookingBook.Domain.Factories;

public class UserFactory : IUserFactory
{
    public User Create(UserId id, UserName name,PasswordHash passwordHash, int roleId)
        => new(id, name,passwordHash, roleId);
}