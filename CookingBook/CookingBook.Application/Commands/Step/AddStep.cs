using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Step;

public record AddStep(Guid RecipeId, string Name) : ICommand;
