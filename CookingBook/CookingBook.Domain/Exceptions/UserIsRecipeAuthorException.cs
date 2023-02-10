using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Domain.Exceptions;

public class UserIsRecipeAuthorException : CookingBookBadRequestException
{
    public UserIsRecipeAuthorException() : base("Author cannot add reviews to his recipes.")
    {
    }
}