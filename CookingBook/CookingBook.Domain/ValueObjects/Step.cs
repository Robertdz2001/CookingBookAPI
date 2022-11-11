using CookingBook.Domain.Exceptions;

namespace CookingBook.Domain.ValueObjects;

public class Step
{
    public string Name { get; set; }
    
    public Step(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new EmptyStepNameException();
        }
        
        Name = name;
    }

    public static implicit operator string(Step name)
        => name.Name;

    public static implicit operator Step(string name)
        => new(name);
}
