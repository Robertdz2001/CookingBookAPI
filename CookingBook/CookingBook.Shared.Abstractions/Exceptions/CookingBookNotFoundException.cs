namespace CookingBook.Shared.Abstractions.Exceptions;

public abstract class CookingBookNotFoundException : CookingBookException
{
    protected CookingBookNotFoundException(string message) : base(message)
    {
    }
}