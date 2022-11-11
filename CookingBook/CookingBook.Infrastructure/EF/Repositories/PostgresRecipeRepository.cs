using CookingBook.Domain;
using CookingBook.Domain.Entities;
using CookingBook.Domain.ValueObjects;
using CookingBook.Infrastructure.EF.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CookingBook.Infrastructure.EF.Repositories;

public class PostgresRecipeRepository : IRecipeRepository
{
    private readonly DbSet<Recipe> _recipes;
    private readonly WriteDbContext _writeDbContext;
    
    public PostgresRecipeRepository(WriteDbContext writeDbContext)
    {
        _recipes = writeDbContext.Recipes;
        _writeDbContext = writeDbContext;
    }
    
    public async Task<Recipe> GetAsync(RecipeId id)
    => await _recipes
        .Include("_ingredients")
        .Include("_steps")
        .Include("_tools")
        .FirstOrDefaultAsync(r => r.Id == id);
        
    

    public async Task AddAsync(Recipe recipe)
    {
        await _recipes.AddAsync(recipe);
        await _writeDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Recipe recipe)
    {
        _recipes.Update(recipe);
        await _writeDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Recipe recipe)
    {
        _recipes.Remove(recipe);
        await _writeDbContext.SaveChangesAsync();
    }
}