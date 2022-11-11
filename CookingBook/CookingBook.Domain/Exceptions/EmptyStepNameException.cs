using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Domain.Exceptions;

public class EmptyStepNameException : CookingBookBadRequestException
{
    public EmptyStepNameException() : base("Step name cannot be empty.")
    {
    }
}