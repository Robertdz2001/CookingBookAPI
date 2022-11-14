using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Application.Exceptions;

public class PasswordsDontMatchException : CookingBookBadRequestException
{
    public PasswordsDontMatchException() : base("Password and confirm password don't match.")
    {
    }
}