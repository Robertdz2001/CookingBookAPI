using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands;

public record RemoveRecipe(Guid Id) : ICommand;
