using CookingBook.Application.Commands.User;
using CookingBook.Application.Exceptions;
using CookingBook.Application.Services;
using CookingBook.Domain;
using CookingBook.Domain.Factories;
using CookingBook.Shared.Abstractions.Commands;
using Microsoft.AspNetCore.Identity;

namespace CookingBook.Application.Commands.Handlers.User;

public class RegisterUserHandler : ICommandHandler<RegisterUser>
{
    private readonly IUserFactory _factory;
    private readonly IUserRepository _repository;
    private readonly IUserReadService _readService;
    private readonly IPasswordHasher<Domain.Entities.User> _passwordHasher;

    public RegisterUserHandler(IUserFactory factory, IUserRepository repository, IUserReadService readService, IPasswordHasher<Domain.Entities.User> passwordHasher)
    {
        _factory = factory;
        _repository = repository;
        _readService = readService;
        _passwordHasher = passwordHasher;
    }

    public async Task HandleAsync(RegisterUser command)
    {
        var alreadyExists = await _readService.ExistsByUserName(command.UserName);

        if (alreadyExists)
        {
            throw new UserAlreadyExistsException(command.UserName);
        }

        if (!command.Password.Equals(command.ConfirmPassword))
        {
            throw new PasswordsDontMatchException();
        }

        var newUser = _factory.Create(command.Id, command.UserName,"placeHolder", 1);
        
            
        var hashedPassword = _passwordHasher.HashPassword(newUser, command.Password);


        newUser.PasswordHash = hashedPassword;

        await _repository.AddAsync(newUser);
        
    }
}