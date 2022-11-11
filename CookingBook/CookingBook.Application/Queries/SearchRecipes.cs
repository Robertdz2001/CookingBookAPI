using CookingBook.Application.DTO;
using CookingBook.Shared.Abstractions.Queries;

namespace CookingBook.Application.Queries;

public class SearchRecipes : IQuery<IEnumerable<RecipeDto>>
{
    public string? SearchPhrase { get; set; }
}