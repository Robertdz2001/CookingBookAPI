using CookingBook.Application.DTO;
using CookingBook.Application.Queries;
using CookingBook.Infrastructure.EF.Contexts;
using CookingBook.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace CookingBook.Infrastructure.EF.Queries.Handlers;

public class GetTopRecipesHandler: IQueryHandler<GetTopRecipes,IEnumerable<RecipeDto>>
{
    private readonly ReadDbContext _context;

    public GetTopRecipesHandler(ReadDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RecipeDto>> HandleAsync(GetTopRecipes query)
        => await _context.Recipes.OrderByDescending(r => r.RecipeRating)
                                 .Take(3)
                                 .Include(r=>r.User)
                                 .Include(r=>r.Ingredients)
                                 .Include(r=>r.Tools)
                                 .Include(r=>r.Steps)
                                 .Include(r=>r.Reviews)
                                 .ThenInclude(r=>r.User)
                                 .Select(r=>r.AsDto())
                                 .ToListAsync();
}