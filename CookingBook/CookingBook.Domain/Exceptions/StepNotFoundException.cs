using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Domain.Exceptions;

public class StepNotFoundException : CookingBookNotFoundException
{
    public StepNotFoundException(string name) : base($"Step with name '{name}' not found.")
    {
    }
}