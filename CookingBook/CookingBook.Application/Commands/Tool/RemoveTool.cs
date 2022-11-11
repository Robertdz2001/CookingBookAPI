using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Tool;

public record RemoveTool(Guid RecipeId, string Name) : ICommand;