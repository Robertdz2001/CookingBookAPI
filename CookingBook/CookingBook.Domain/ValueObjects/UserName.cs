using CookingBook.Domain.Exceptions;

namespace CookingBook.Domain.ValueObjects;

public class UserName
{
    public string Value { get; }

    public UserName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyUserNameException();
        }
      
        Value = value;
    }

    public static implicit operator string(UserName name)
        => name.Value;
   
    public static implicit operator UserName(string name)
        => new(name);
}