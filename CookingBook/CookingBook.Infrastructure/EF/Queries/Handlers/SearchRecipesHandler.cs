using CookingBook.Application.DTO;
using CookingBook.Application.Queries;
using CookingBook.Infrastructure.EF.Contexts;
using CookingBook.Infrastructure.EF.Models;
using CookingBook.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace CookingBook.Infrastructure.EF.Queries.Handlers;

public class SearchRecipesHandler : IQueryHandler<SearchRecipes,IEnumerable<RecipeDto>>
{
    private readonly DbSet<RecipeReadModel> _recipes;

    public SearchRecipesHandler(ReadDbContext readDbContext)
    {
        _recipes = readDbContext.Recipes;
    }
    public async Task<IEnumerable<RecipeDto>> HandleAsync(SearchRecipes query)
    {
        var dbQuery = _recipes
            .Include(r => r.Ingredients)
            .Include(r => r.Steps)
            .Include(r => r.Tools)
            .AsQueryable();
        
        if (query.SearchPhrase is not null)
        {
            dbQuery = dbQuery.Where(r => r.Name.Contains(query.SearchPhrase));
        }

        return await dbQuery
            .Select(r=>r.AsDto())
            .ToListAsync();
    }
}