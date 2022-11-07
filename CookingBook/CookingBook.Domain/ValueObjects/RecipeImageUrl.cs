namespace CookingBook.Domain.ValueObjects;

public record RecipeImageUrl(string Value)
{

    public static implicit operator string(RecipeImageUrl url)
        => url.Value;

    public static implicit operator RecipeImageUrl(string url)
        => new(url);
}