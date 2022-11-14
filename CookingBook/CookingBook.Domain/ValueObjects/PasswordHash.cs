using CookingBook.Domain.Exceptions;

namespace CookingBook.Domain.ValueObjects;

public record PasswordHash
{
    public string Value { get; }

    public PasswordHash(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyPasswordHashException();
        }
      
        Value = value;
    }

    public PasswordHash()
    {
        
    }

    public static implicit operator string(PasswordHash passwordHash)
        => passwordHash.Value;
   
    public static implicit operator PasswordHash(string passwordHash)
        => new(passwordHash);
}