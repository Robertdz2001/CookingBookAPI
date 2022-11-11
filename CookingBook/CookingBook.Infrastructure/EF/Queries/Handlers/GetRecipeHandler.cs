using CookingBook.Application.DTO;
using CookingBook.Application.Queries;
using CookingBook.Infrastructure.EF.Contexts;
using CookingBook.Infrastructure.EF.Models;
using CookingBook.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace CookingBook.Infrastructure.EF.Queries.Handlers;

public class GetRecipeHandler : IQueryHandler<GetRecipe,RecipeDto>
{
    private readonly DbSet<RecipeReadModel> _recipes;

    public GetRecipeHandler(ReadDbContext readDbContext)
    {
        _recipes = readDbContext.Recipes;
    }

    public async Task<RecipeDto> HandleAsync(GetRecipe query)
        => await _recipes
            .Include(r => r.Ingredients)
            .Include(r => r.Steps)
            .Include(r => r.Tools)
            .Where(r=>r.Id==query.Id)
            .Select(r => r.AsDto())
            .FirstOrDefaultAsync();
}