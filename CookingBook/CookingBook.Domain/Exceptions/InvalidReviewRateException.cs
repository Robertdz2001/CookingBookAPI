using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Domain.Exceptions;

public class InvalidReviewRateException : CookingBookBadRequestException
{
    public InvalidReviewRateException() : base("Review rate has to be between -5 and 5.")
    {
    }
}