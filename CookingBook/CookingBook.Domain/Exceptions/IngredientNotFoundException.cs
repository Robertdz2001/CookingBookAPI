using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Domain.Exceptions;

public class IngredientNotFoundException : CookingBookNotFoundException
{
    public IngredientNotFoundException(string name) : base($"Ingredient with name '{name}' not found.")
    {
    }
}