using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Step;

public record ChangeStep(Guid RecipeId, string StepToChangeName, string Name) : ICommand;