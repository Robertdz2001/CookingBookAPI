using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Tool;

public record AddTool(Guid RecipeId, string Name, ushort Quantity) : ICommand;
