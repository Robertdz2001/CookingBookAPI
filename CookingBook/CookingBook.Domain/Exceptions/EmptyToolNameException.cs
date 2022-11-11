using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Domain.Exceptions;

public class EmptyToolNameException : CookingBookBadRequestException
{
    public EmptyToolNameException() : base("Tool name cannot be empty.")
    {
    }
}