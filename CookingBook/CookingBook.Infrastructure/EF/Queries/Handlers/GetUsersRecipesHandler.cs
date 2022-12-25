using CookingBook.Application.DTO;
using CookingBook.Application.Queries;
using CookingBook.Application.Services;
using CookingBook.Infrastructure.EF.Contexts;
using CookingBook.Infrastructure.EF.Models;
using CookingBook.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace CookingBook.Infrastructure.EF.Queries.Handlers;

public class GetUsersRecipesHandler : IQueryHandler<GetUsersRecipes, IEnumerable<RecipeDto>>
{
    private readonly DbSet<RecipeReadModel> _recipes;
    private readonly IUserContextService _userContext;

    public GetUsersRecipesHandler(ReadDbContext readDbContext, IUserContextService userContext)
    {
        _userContext = userContext;
        _recipes = readDbContext.Recipes;
    }

    public async Task<IEnumerable<RecipeDto>> HandleAsync(GetUsersRecipes query)
        => await _recipes
            .Include(r => r.Ingredients)
            .Include(r => r.Steps)
            .Include(r => r.Tools)
            .Include(r=>r.Reviews)
            .ThenInclude(r=>r.User)
            .Where(r=>r.UserId==_userContext.GetUserId)
            .Select(r => r.AsDto())
            .ToListAsync();
}