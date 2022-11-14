using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Domain.Exceptions;

public class EmptyPasswordHashException : CookingBookBadRequestException
{
    public EmptyPasswordHashException() : base("PasswordHash cannot be empty.")
    {
    }
}