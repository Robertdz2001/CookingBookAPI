using CookingBook.Domain.Exceptions;

namespace CookingBook.Domain.ValueObjects;

public class Step
{
    public string Value { get; set; }
    
    public Step(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyStepNameException();
        }
        
        Value = value;
    }

    public static implicit operator string(Step name)
        => name.Value;

    public static implicit operator Step(string name)
        => new(name);
}
