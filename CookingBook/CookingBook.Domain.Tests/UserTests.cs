using CookingBook.Domain.Entities;
using CookingBook.Domain.Exceptions;
using CookingBook.Domain.ValueObjects;
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
        UserConstructor_Throws_EmptyPasswordHashException_When_PasswordHash_Is_Empty()
    {
        var exception = Record.Exception(() => new User(Guid.NewGuid(), "UserName", ""));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<EmptyPasswordHashException>();
    }
    
    
    [Fact]
    public void
        UserConstructor_Creates_New_User_When_Proper_Parameters_Are_Given()
    {
        User user = new User(Guid.NewGuid(), "UserName123", "PasswordHash123");
        
        var exception = Record.Exception(() => user = new User(Guid.NewGuid(), "UserName", "PasswordHash"));

        exception.ShouldBeNull();
        user.PasswordHash.ShouldBe((PasswordHash)"PasswordHash");
    }
}