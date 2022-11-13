using CookingBook.Domain.Consts;
using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.User;

public record ChangeUserRole(Guid Id, Role Role) : ICommand;
