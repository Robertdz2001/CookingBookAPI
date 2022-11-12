using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Domain.Exceptions;

public class EmptyUserNameException : CookingBookBadRequestException
{
    public EmptyUserNameException() : base("UserName cannot be empty.")
    {
    }
}