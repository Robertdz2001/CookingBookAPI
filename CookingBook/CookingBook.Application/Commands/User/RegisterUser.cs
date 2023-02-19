using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.User;

public record RegisterUser(Guid Id, string UserName, string Password,string ImageUrl, string ConfirmPassword) : ICommand;
