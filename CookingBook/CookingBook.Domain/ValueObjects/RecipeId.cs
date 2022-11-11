using CookingBook.Domain.Exceptions;

namespace CookingBook.Domain.ValueObjects;

public record RecipeId
{
    public Guid Value { get; }

    public RecipeId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new EmptyRecipeIdException();
        }
        
        Value = value;
    }
    
    public static implicit operator Guid(RecipeId id)
        => id.Value;
    
    public static implicit operator RecipeId(Guid id)
        => new(id);

}