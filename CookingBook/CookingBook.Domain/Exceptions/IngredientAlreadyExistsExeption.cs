using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Domain.Exceptions;

public class IngredientAlreadyExistsExeption : CookingBookBadRequestException
{
    public IngredientAlreadyExistsExeption(string name) : base($"Ingredient with name : '{name}' already exists.")
    {
    }
}