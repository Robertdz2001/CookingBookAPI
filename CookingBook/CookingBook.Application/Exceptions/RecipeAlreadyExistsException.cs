using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Application.Exceptions;

public class RecipeAlreadyExistsException : CookingBookBadRequestException
{
    public RecipeAlreadyExistsException(Guid id) : base($"Recipe with id: '{id}' already exists.")
    {
    }
}