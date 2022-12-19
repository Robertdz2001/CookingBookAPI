using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Domain.Exceptions;

public class EmptyReviewContentException : CookingBookBadRequestException
{
    public EmptyReviewContentException() : base("Review content cannot be empty.")
    {
    }
}