using CookingBook.Domain.Exceptions;

namespace CookingBook.Domain.ValueObjects;

public class Tool
{
    public string Name { get; set; }
    public ushort Quantity { get; set; }

    public Tool(string name, ushort quantity)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new EmptyToolNameException();
        }
        Name = name;
        Quantity = quantity;
    }
}