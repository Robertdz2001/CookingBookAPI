using CookingBook.Application.Commands.Handlers.User;
using CookingBook.Application.Commands.User;
using CookingBook.Application.Exceptions;
using CookingBook.Application.Services;
using CookingBook.Domain;
using CookingBook.Domain.Factories;
using CookingBook.Shared.Abstractions.Commands;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Shouldly;

namespace CookingBook.Application.Tests.User;

public class RegisterUserHandlerTests
{


    [Fact]
    public async Task
        HandleAsync_Throws_UserAlreadyExistsException_When_There_Is_Already_User_With_The_Same_Name()
    {
        var command = new RegisterUser(Guid.NewGuid(), "UserName", "Password","Url", "Password");

        _readService.ExistsByUserName(command.UserName).Returns(true);

        var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));

        exception.ShouldNotBeNull();
        
        exception.ShouldBeOfType<UserAlreadyExistsException>();
    }
    
    [Fact]
    public async Task
        HandleAsync_Throws_PasswordsDontMatchException_When_Password_And_ConfirmPassword_Do_Not_Match()
    {
        var command = new RegisterUser(Guid.NewGuid(), "UserName", "Password","Url", "Password1");

        _readService.ExistsByUserName(command.UserName).Returns(false);

        var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));

        exception.ShouldNotBeNull();
        
        exception.ShouldBeOfType<PasswordsDontMatchException>();
    }
    
    [Fact]
    public async Task
        HandleAsync_Calls_Repository_On_Success()
    {
        var command = new RegisterUser(Guid.NewGuid(), "UserName", "Password","Url", "Password");
        
        var user = new Domain.Entities.User(command.Id, command.UserName,"Url", "placeHolder");
        
        _readService.ExistsByUserName(command.UserName).Returns(false);

        _factory.Create(command.Id, command.UserName, "placeHolder", "Url",1).ReturnsForAnyArgs(user);
        
        _passwordHasher.HashPassword(user, command.Password).Returns("PasswordHash");

        var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));

        exception.ShouldBeNull();

       await _repository.Received(1).AddAsync(Arg.Any<Domain.Entities.User>());
    }




    #region ARRANGE

    private readonly IUserFactory _factory;
    private readonly IUserRepository _repository;
    private readonly IUserReadService _readService;
    private readonly IPasswordHasher<Domain.Entities.User> _passwordHasher;
    private readonly ICommandHandler<RegisterUser> _commandHandler; 

    public RegisterUserHandlerTests()
    {
        _factory = Substitute.For<IUserFactory>();
        _repository = Substitute.For<IUserRepository>();
        _readService = Substitute.For<IUserReadService>();
        _passwordHasher=Substitute.For<IPasswordHasher<Domain.Entities.User>>();;
        _commandHandler = new RegisterUserHandler(_factory, _repository, _readService,
            _passwordHasher);
    }
    
    #endregion
}