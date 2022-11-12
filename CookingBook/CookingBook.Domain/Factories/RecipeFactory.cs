using CookingBook.Domain.Entities;
using CookingBook.Domain.ValueObjects;

namespace CookingBook.Domain.Factories;

public class RecipeFactory : IRecipeFactory
{
    public Recipe Create(UserId userId ,RecipeId id, RecipeName name, RecipeImageUrl url, RecipePrepTime prepTime)
        => new(userId,id, name, url, prepTime, DateTime.UtcNow);



}