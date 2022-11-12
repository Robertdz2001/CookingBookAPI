using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Application.Exceptions;

public class UserAlreadyExistsException : CookingBookBadRequestException
{
    public UserAlreadyExistsException(string userName)  : base($"User with userName: '{userName}' already exists.")
    {
    }
}