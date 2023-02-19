namespace CookingBook.Domain.ValueObjects;

public record ImageUrl(string Value)
{

    public static implicit operator string(ImageUrl url)
        => url.Value;

    public static implicit operator ImageUrl(string url)
        => new(url);
}