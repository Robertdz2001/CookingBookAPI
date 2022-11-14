using CookingBook.Domain.Entities;
using CookingBook.Domain.ValueObjects;

namespace CookingBook.Domain.Factories;

public interface IRecipeFactory
{
    public Recipe Create(UserId userId ,RecipeId id, RecipeName name, RecipeImageUrl url, RecipePrepTime prepTime);
}