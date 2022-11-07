using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Domain.Exceptions;

public class EmptyIngredientNameException : CookingBookBadRequestException
{
    public EmptyIngredientNameException() : base("Ingredient name cannot be empty.")
    {
    }
}