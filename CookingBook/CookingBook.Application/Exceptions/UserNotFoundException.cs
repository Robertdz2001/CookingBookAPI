using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Application.Exceptions;

public class UserNotFoundException : CookingBookNotFoundException
{
    public UserNotFoundException(Guid id) : base($"User with id: '{id}' not found.")
    {
    }
}