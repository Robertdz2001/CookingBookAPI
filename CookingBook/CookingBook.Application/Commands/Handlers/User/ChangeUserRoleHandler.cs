using CookingBook.Application.Commands.User;
using CookingBook.Application.Exceptions;
using CookingBook.Domain;
using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Handlers.User;

public class ChangeUserRoleHandler : ICommandHandler<ChangeUserRole>
{
    
    private readonly IUserRepository _repository;

    public ChangeUserRoleHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(ChangeUserRole command)
    {
        var user = await _repository.GetAsync(command.Id);

        if (user is null)
        {
            throw new UserNotFoundException(command.Id);
        }

        user.SetUserRole(command.RoleId);

        await _repository.UpdateAsync(user);
    }
}