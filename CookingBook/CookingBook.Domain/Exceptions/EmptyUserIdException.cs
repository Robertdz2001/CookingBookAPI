using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Domain.Exceptions;

public class EmptyUserIdException : CookingBookBadRequestException
{
    public EmptyUserIdException() : base("User id cannot be empty.")
    {
    }
}