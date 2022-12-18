using CookingBook.Application.DTO;
using CookingBook.Infrastructure.EF.Models;
using CookingBook.Shared.Abstractions.Queries;

namespace CookingBook.Application.Queries;

public class SearchRecipes : IQuery<PagedResult<RecipeDto>>
{
    public string? SearchPhrase { get; set; }
    public SortByOptions? SortBy { get; set; }
    public bool SortByDescending { get; set; } = false;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 5;


}

public enum SortByOptions
{
    Name,
    PrepTime,
    Calories,
    CreatedDate
}