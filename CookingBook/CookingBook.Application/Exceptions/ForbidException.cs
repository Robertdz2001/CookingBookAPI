using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Application.Exceptions;

public class ForbidException : CookingBookException
{
    public ForbidException() : base("You are unauthorized to do this.")
    {
    }
}