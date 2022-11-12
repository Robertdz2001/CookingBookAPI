using CookingBook.Domain.Consts;
using CookingBook.Domain.Entities;
using CookingBook.Domain.ValueObjects;

namespace CookingBook.Domain.Factories;

public class UserFactory : IUserFactory
{
    public User Create(UserId id, UserName name,PasswordHash passwordHash, Role userRole)
        => new(id, name,passwordHash, userRole);
}