using CookingBook.Shared.Abstractions.Exceptions;

namespace CookingBook.Domain.Exceptions;

public class ToolNotFoundException : CookingBookNotFoundException
{
    public ToolNotFoundException(string name) : base($"Tool with name '{name}' not found.")
    {
    }
}