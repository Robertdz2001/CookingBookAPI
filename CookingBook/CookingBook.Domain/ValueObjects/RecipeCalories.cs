namespace CookingBook.Domain.ValueObjects;

public record RecipeCalories(ushort Value)
{
    public static implicit operator ushort(RecipeCalories kcal)
        => kcal.Value;

    public static implicit operator RecipeCalories(ushort kcal)
        => new(kcal);
}