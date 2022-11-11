using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Tool;

public record ChangeTool(Guid RecipeId,string ToolToChangeName, string Name, ushort Quantity) : ICommand;