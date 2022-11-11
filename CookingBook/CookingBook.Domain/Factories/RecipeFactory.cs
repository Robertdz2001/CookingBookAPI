using CookingBook.Domain.Entities;
using CookingBook.Domain.ValueObjects;

namespace CookingBook.Domain.Factories;

public class RecipeFactory : IRecipeFactory
{
    public Recipe Create(RecipeId id, RecipeName name, RecipeImageUrl url, RecipePrepTime prepTime)
        => new(id, name, url, prepTime, DateTime.UtcNow);



}