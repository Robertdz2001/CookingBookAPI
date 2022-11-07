namespace CookingBook.Shared.Abstractions.Exceptions;

public abstract class CookingBookException : Exception
{
    protected CookingBookException(string message) : base(message)
    {
    }
}