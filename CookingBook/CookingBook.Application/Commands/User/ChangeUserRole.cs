using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.User;

public record ChangeUserRole(Guid Id, int RoleId) : ICommand;
