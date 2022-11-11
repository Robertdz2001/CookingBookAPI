namespace CookingBook.Domain.ValueObjects;

public record RecipeCalories(double Value)
{
    public static implicit operator double(RecipeCalories kcal)
        => kcal.Value;

    public static implicit operator RecipeCalories(double kcal)
        => new(Math.Abs(kcal));
}