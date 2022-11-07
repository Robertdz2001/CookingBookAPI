namespace CookingBook.Shared.Abstractions.Exceptions;

public class CookingBookBadRequestException : CookingBookException
{
    public CookingBookBadRequestException(string message) : base(message)
    {
    }
}