namespace CookingBook.Domain.ValueObjects;

public record RecipeRating(short Value)
{
    public static implicit operator short(RecipeRating rating) => rating.Value;
    
    public static implicit operator RecipeRating(short value) => new(value);
}