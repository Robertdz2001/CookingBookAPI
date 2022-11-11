using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands;

public record CreateRecipe(Guid Id, string Name, string ImageUrl, ushort PrepTime) : ICommand;
