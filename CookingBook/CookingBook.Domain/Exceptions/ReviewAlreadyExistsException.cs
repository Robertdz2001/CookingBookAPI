using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Domain.Exceptions;

public class ReviewAlreadyExistsException : CookingBookBadRequestException
{
    public ReviewAlreadyExistsException(string message) : base(message)
    {
    }
}