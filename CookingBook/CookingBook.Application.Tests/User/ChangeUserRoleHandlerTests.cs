using CookingBook.Application.Commands.Handlers.User;
using CookingBook.Application.Commands.User;
using CookingBook.Application.Exceptions;
using CookingBook.Domain;
using CookingBook.Shared.Abstractions.Commands;
using NSubstitute;
using Shouldly;

namespace CookingBook.Application.Tests.User;

public class ChangeUserRoleHandlerTests
{

    [Fact]
    public async Task
        HandleAsync_Throws_UserNotFoundException_When_There_Is_No_User_With_Given_Id()
    {
        var command = new ChangeUserRole(Guid.NewGuid(), 1);

        _repository.GetAsync(command.Id).Returns(default(Domain.Entities.User));

        var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));

        exception.ShouldNotBeNull();

        exception.ShouldBeOfType<UserNotFoundException>();



    }
    
    [Fact]
    public async Task
        HandleAsync_Calls_Repository_On_Success()
    {
        var command = new ChangeUserRole(Guid.NewGuid(), 1);
    
        var user = new Domain.Entities.User(command.Id, "UserName", "PasswordHash");
    
        _repository.GetAsync(command.Id).Returns(user);
        
        var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));
    
        exception.ShouldBeNull();

        await _repository.Received(1).UpdateAsync(Arg.Any<Domain.Entities.User>());

    }
    
    
    
    
    
    
    private readonly IUserRepository _repository;
    private readonly ICommandHandler<ChangeUserRole> _commandHandler;

    public ChangeUserRoleHandlerTests()
    {
        _repository = Substitute.For<IUserRepository>();
        _commandHandler = new ChangeUserRoleHandler(_repository);
    }
}