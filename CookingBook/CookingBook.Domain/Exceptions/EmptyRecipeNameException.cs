using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Domain.Exceptions;

public class EmptyRecipeNameException : CookingBookBadRequestException
{
    public EmptyRecipeNameException() : base("Recipe name cannot be empty.")
    {
    }
}