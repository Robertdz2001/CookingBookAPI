using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Domain.Exceptions;

public class ToolAlreadyExistsException : CookingBookBadRequestException
{
    public ToolAlreadyExistsException(string name) : base($"Ingredient with name : '{name}' already exists.")
    {
    }
}