namespace CookingBook.Domain.ValueObjects;

public record RecipeCreatedDate(DateTime Value)
{
    public static implicit operator DateTime(RecipeCreatedDate date)
        => new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, date.Value.Hour,date.Value.Minute,date.Value.Second);

    public static implicit operator RecipeCreatedDate(DateTime date)
        => new(date);
}