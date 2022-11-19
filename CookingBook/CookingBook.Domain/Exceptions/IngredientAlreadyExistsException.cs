using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Domain.Exceptions;

public class IngredientAlreadyExistsException : CookingBookBadRequestException
{
    public IngredientAlreadyExistsException(string name) : base($"Ingredient with name : '{name}' already exists.")
    {
    }
}