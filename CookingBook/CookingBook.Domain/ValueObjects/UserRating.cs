namespace CookingBook.Domain.ValueObjects;

public class UserRating
{
       
    public short Value { get; set; }

    public UserRating(short value)
    {
        Value = value;
    }
    public static implicit operator short(UserRating rating) => rating.Value;
    
    public static implicit operator UserRating(short value) => new(value);
}