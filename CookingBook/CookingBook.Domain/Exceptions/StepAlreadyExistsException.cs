using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Domain.Exceptions;

public class StepAlreadyExistsException : CookingBookBadRequestException
{
    public StepAlreadyExistsException(string name) : base($"Step with name : '{name}' already exists.")
    {
    }
}