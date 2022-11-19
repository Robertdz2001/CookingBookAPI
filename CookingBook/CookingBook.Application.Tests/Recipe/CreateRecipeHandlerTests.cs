using CookingBook.Application.Commands;
using CookingBook.Application.Commands.Handlers;
using CookingBook.Application.Exceptions;
using CookingBook.Application.Services;
using CookingBook.Domain;
using CookingBook.Domain.Factories;
using CookingBook.Shared.Abstractions.Commands;
using NSubstitute;
using Shouldly;

namespace CookingBook.Application.Tests.Recipe;

public class CreateRecipeHandlerTests
{


    [Fact]
    public async Task
        HandleAsync_Throws_RecipeAlreadyExistsException_When_There_Is_Already_Recipe_With_The_Same_Id()
    {
        var command = new CreateRecipe(Guid.NewGuid(), "Recipe", "Url", 30);

        _readService.ExistsById(command.Id).Returns(true);

        var exception = await Record.ExceptionAsync(() =>_commandHandler.HandleAsync(command));

        exception.ShouldNotBeNull();

        exception.ShouldBeOfType<RecipeAlreadyExistsException>();
        
    }
    
    [Fact]
    public async Task
        HandleAsync_Calls_Repository_On_Success()
    {
        var command = new CreateRecipe(Guid.NewGuid(), "Recipe", "Url", 30);

        _readService.ExistsById(command.Id).Returns(false);

        _userContext.GetUserId.Returns(Guid.NewGuid());

        _factory.Create(_userContext.GetUserId, command.Id, command.Name, command.ImageUrl, command.PrepTime).Returns(default(Domain.Entities.Recipe));

        var exception = await Record.ExceptionAsync(() =>_commandHandler.HandleAsync(command));

        exception.ShouldBeNull();

        await _repository.Received(1).AddAsync(Arg.Any<Domain.Entities.Recipe>());
        
    }







    #region ARRANGE
    private readonly IRecipeFactory _factory;
    private readonly IRecipeRepository _repository;
    private readonly IRecipeReadService _readService;
    private readonly IUserContextService _userContext;
    private readonly ICommandHandler<CreateRecipe> _commandHandler;
    public CreateRecipeHandlerTests()
    {
        
        _factory = Substitute.For<IRecipeFactory>();
        _repository = Substitute.For<IRecipeRepository>();
        _readService = Substitute.For<IRecipeReadService>();
        _userContext = Substitute.For<IUserContextService>();
        
        _commandHandler = new CreateRecipeHandler(_factory,_repository,_readService,_userContext);
        
        
    }
    #endregion
}