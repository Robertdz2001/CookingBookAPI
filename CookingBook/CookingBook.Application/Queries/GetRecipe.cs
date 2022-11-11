using CookingBook.Application.DTO;
using CookingBook.Shared.Abstractions.Queries;

namespace CookingBook.Application.Queries;

public class GetRecipe : IQuery<RecipeDto>
{
    public Guid Id { get; set; }
}