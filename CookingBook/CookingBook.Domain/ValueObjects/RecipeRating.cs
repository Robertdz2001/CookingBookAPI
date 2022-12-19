namespace CookingBook.Domain.ValueObjects;

public class RecipeRating
{
    
    public short Value { get; set; }

    public RecipeRating(short value)
    {
        Value = value;
    }
    public static implicit operator short(RecipeRating rating) => rating.Value;
    
    public static implicit operator RecipeRating(short value) => new(value);
}