using System.Linq.Expressions;
using CookingBook.Application.DTO;
using CookingBook.Application.Queries;
using CookingBook.Infrastructure.EF.Contexts;
using CookingBook.Infrastructure.EF.Models;
using CookingBook.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace CookingBook.Infrastructure.EF.Queries.Handlers;

public class SearchRecipesHandler : IQueryHandler<SearchRecipes,PagedResult<RecipeDto>>
{
    private readonly DbSet<RecipeReadModel> _recipes;

    public SearchRecipesHandler(ReadDbContext readDbContext)
    {
        _recipes = readDbContext.Recipes;
    }
    public async Task<PagedResult<RecipeDto>> HandleAsync(SearchRecipes query)
    {
        var dbQuery = _recipes
            .Include(r=>r.User)
            .Include(r => r.Ingredients)
            .Include(r => r.Steps)
            .Include(r => r.Tools)
            .Include(r=>r.Reviews)
            .ThenInclude(r=>r.User)
            .AsQueryable();

        
        
        if (query.SearchByRecipeName is not null)
        {
            dbQuery = dbQuery.Where(r => r.Name.Contains(query.SearchByRecipeName));
        }
        
        if (query.SearchByUserName is not null)
        {
            dbQuery = dbQuery.Where(r => r.User.UserName.Contains(query.SearchByUserName));
        }
        
        var totalCount = dbQuery.Count();
        
        var columnsSelector = new Dictionary<SortByOptions?, Expression<Func<RecipeReadModel, object>>>
            {
                { SortByOptions.Name , recipe=>recipe.Name},
                { SortByOptions.Calories , recipe=>recipe.Calories},
                { SortByOptions.PrepTime , recipe=>recipe.PrepTime},
                { SortByOptions.CreatedDate , recipe=>recipe.CreatedDate},
                { SortByOptions.Rating , recipe=>recipe.RecipeRating}
            };

            var sortByExpression = columnsSelector[query.SortBy];

            dbQuery = query.SortByDescending ? dbQuery.OrderByDescending(sortByExpression) : dbQuery.OrderBy(sortByExpression);
        

        var result = await dbQuery
                                            .Skip(query.PageSize * (query.PageNumber - 1))
                                            .Take(query.PageSize)
                                            .Select(r=>r.AsDto())
                                            .ToListAsync();

        var pagedResult = new PagedResult<RecipeDto>(result, totalCount, query.PageSize, query.PageNumber);

        return pagedResult;
    }
}