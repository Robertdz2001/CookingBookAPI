using CookingBook.Domain.Entities;
using CookingBook.Domain.ValueObjects;

namespace CookingBook.Domain.Factories;

public interface IUserFactory
{
   public User Create(UserId id, UserName name,PasswordHash passwordHash ,int roleId);
}