namespace CookingBook.Domain.ValueObjects;

public record RecipePrepTime(ushort Value)
{
    public static implicit operator ushort(RecipePrepTime time)
        => time.Value;
    public static implicit operator RecipePrepTime(ushort time)
        => new(time);
}