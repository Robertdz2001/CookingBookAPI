using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Step;

public record RemoveStep(Guid RecipeId, string Name) : ICommand;