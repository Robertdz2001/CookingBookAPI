using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Domain.Exceptions;

public class EmptyReviewNameException : CookingBookBadRequestException
{
    public EmptyReviewNameException() : base("Review name cannot be empty.")
    {
    }
}