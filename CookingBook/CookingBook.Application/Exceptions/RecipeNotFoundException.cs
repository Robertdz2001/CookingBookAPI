using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Application.Exceptions;

public class RecipeNotFoundException : CookingBookNotFoundException
{
    public RecipeNotFoundException(Guid id) : base($"Recipe with id: '{id}' not found.")
    {
    }
}