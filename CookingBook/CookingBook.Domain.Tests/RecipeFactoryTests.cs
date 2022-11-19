using CookingBook.Domain.Entities;
using CookingBook.Domain.Exceptions;
using CookingBook.Domain.Factories;
using CookingBook.Domain.ValueObjects;
using Shouldly;

namespace CookingBook.Domain.Tests;

public class RecipeFactoryTests
{
    [Fact]
    public void Create_Throws_EmptyUserIdException_When_UserId_Is_Empty()
    {

        var exception = Record.Exception(() => _factory.Create(Guid.Empty, Guid.NewGuid(), "Name", "Url", 32));

        exception.ShouldNotBeNull();

        exception.ShouldBeOfType<EmptyUserIdException>();
    }
    
    [Fact]
    public void Create_Throws_EmptyRecipeIdException_When_RecipeId_Is_Empty()
    {

        var exception = Record.Exception(() => _factory.Create(Guid.NewGuid(), Guid.Empty, "Name", "Url", 32));

        exception.ShouldNotBeNull();

        exception.ShouldBeOfType<EmptyRecipeIdException>();
    }
    
    [Fact]
    public void Create_Throws_EmptyRecipeNameException_When_RecipeId_Is_Empty()
    {

        var exception = Record.Exception(() => _factory.Create(Guid.NewGuid(), Guid.NewGuid(), "", "Url", 32));

        exception.ShouldNotBeNull();

        exception.ShouldBeOfType<EmptyRecipeNameException>();
    }
    
    [Fact]
    public void Create_Creates_New_Recipe_When_Proper_Parameters_Are_Given()
    {
        var uid = Guid.NewGuid();
        var recipe = new Recipe(Guid.NewGuid(), Guid.NewGuid(), "Name", "Url", 32, DateTime.UtcNow);

        var exception = Record.Exception(() =>recipe = _factory.Create(uid, Guid.NewGuid(), "Name", "Url", 32));

        exception.ShouldBeNull();

        recipe.UserId.ShouldBe((UserId)uid);
    }
    
    
    
    
    
    
    
    private readonly IRecipeFactory _factory;

    public RecipeFactoryTests()
    {
        _factory = new RecipeFactory();
    }
}