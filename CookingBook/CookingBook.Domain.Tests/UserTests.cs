using CookingBook.Domain.Entities;
using CookingBook.Domain.Exceptions;
using Shouldly;

namespace CookingBook.Domain.Tests;

public class UserTests
{
    [Fact]
    public void
        UserConstructor_Throws_EmptyUserIdException_When_UserId_Is_Empty()
    {
        var exception = Record.Exception(() => new User(Guid.Empty, "UserName", "PasswordHash"));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<EmptyUserIdException>();
    }
    
    [Fact]
    public void
        UserConstructor_Throws_EmptyUserNameException_When_UserName_Is_Empty()
    {
        var exception = Record.Exception(() => new User(Guid.NewGuid(), "", "PasswordHash"));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<EmptyUserNameException>();
    }
    
    [Fact]
    public void
        UserConstructor_Throws_EmptyPasswordHashException_When_UserName_Is_Empty()
    {
        var exception = Record.Exception(() => new User(Guid.NewGuid(), "UserName", ""));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<EmptyPasswordHashException>();
    }
}