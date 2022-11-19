using CookingBook.Domain.Consts;
using CookingBook.Domain.Entities;
using CookingBook.Domain.Exceptions;
using CookingBook.Domain.Factories;
using CookingBook.Domain.ValueObjects;
using Shouldly;

namespace CookingBook.Domain.Tests;

public class UserFactoryTests
{
    [Fact]
    public void
        Create_Throws_EmptyUserIdException_When_UserId_Is_Empty()
    {
        var exception = Record.Exception(() => _factory.Create(Guid.Empty, "UserName", "PasswordHash",Role.User));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<EmptyUserIdException>();
    }
    
    [Fact]
    public void
        Create_Throws_EmptyUserNameException_When_UserName_Is_Empty()
    {
        var exception = Record.Exception(() => _factory.Create(Guid.NewGuid(), "", "PasswordHash",Role.User));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<EmptyUserNameException>();
    }
    
    [Fact]
    public void
        Create_Throws_EmptyPasswordHashException_When_PasswordHash_Is_Empty()
    {
        var exception = Record.Exception(() => _factory.Create(Guid.NewGuid(), "UserName", "",Role.User));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<EmptyPasswordHashException>();
    }
    
    
    [Fact]
    public void
        Create_Creates_New_User_When_Proper_Parameters_Are_Given()
    {
        User user = new User(Guid.NewGuid(), "UserName123", "PasswordHash123");
        
        var exception = Record.Exception(() => user = _factory.Create(Guid.NewGuid(), "UserName", "PasswordHash",Role.User));

        exception.ShouldBeNull();
        user.PasswordHash.ShouldBe((PasswordHash)"PasswordHash");
    }

    private readonly IUserFactory _factory;

    public UserFactoryTests()
    {
        _factory = new UserFactory();
    }
}