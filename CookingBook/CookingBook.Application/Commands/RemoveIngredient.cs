using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Handlers;

public record RemoveIngredient(Guid RecipeId, string Name) : ICommand;
