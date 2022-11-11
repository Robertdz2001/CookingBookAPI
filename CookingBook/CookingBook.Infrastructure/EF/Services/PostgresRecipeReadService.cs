using CookingBook.Application.Services;
using CookingBook.Infrastructure.EF.Contexts;
using CookingBook.Infrastructure.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace CookingBook.Infrastructure.EF.Services;

public class PostgresRecipeReadService : IRecipeReadService
{
    private readonly DbSet<RecipeReadModel> _recipes;

    public PostgresRecipeReadService(ReadDbContext readDbContext)
    {
        _recipes = readDbContext.Recipes;
    }

    public async Task<bool> ExistsById(Guid id)
        => await _recipes.AnyAsync(r => r.Id == id);
}