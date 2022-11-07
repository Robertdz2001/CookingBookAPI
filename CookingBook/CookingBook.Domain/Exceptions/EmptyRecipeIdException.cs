using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Domain.Exceptions;

public class EmptyRecipeIdException : CookingBookBadRequestException
{
    public EmptyRecipeIdException() : base("Recipe id cannot be empty")
    {
    }
}