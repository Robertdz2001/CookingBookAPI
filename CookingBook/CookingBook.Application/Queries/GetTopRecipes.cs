using CookingBook.Application.DTO;
using CookingBook.Shared.Abstractions.Queries;

namespace CookingBook.Application.Queries;

public class GetTopRecipes : IQuery<IEnumerable<RecipeDto>>{}
