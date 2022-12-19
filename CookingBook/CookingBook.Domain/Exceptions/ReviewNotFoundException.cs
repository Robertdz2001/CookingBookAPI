using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Domain.Exceptions;

public class ReviewNotFoundException : CookingBookNotFoundException
{
    public ReviewNotFoundException(string name) : base($"Review with name '{name}' not found.")
    {
    }
}