using CookingBook.Domain.Entities;
using CookingBook.Domain.ValueObjects;

namespace CookingBook.Domain;

public interface IRecipeRepository
{
    Task<Recipe> GetAsync(RecipeId id);
    
    Task AddAsync(Recipe recipe);
    
    Task UpdateAsync(Recipe recipe);
    
    Task DeleteAsync(Recipe recipe);
}