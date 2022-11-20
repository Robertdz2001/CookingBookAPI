using System.Security.Claims;
using CookingBook.Application.Commands.Handlers.Step;
using CookingBook.Application.Commands.Step;
using CookingBook.Application.Commands.Tool;
using CookingBook.Application.Exceptions;
using CookingBook.Application.Services;
using CookingBook.Domain;
using CookingBook.Shared.Abstractions.Commands;
using Microsoft.AspNetCore.Authorization;
using NSubstitute;
using Shouldly;

namespace CookingBook.Application.Tests.Step;

public class AddStepHandlerTests
{
    [Fact]
    public async Task
        HandleAsync_Throws_RecipeNotFoundException_When_There_Is_No_Recipe_With_Given_Id()
    {
        var command = new AddStep(Guid.NewGuid(), "Step");
    
        _repository.GetAsync(command.RecipeId).Returns(default(Domain.Entities.Recipe));
    
        var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));
    
        exception.ShouldNotBeNull();
    
        exception.ShouldBeOfType<RecipeNotFoundException>();
    
    }
    
    [Fact]
    public async Task
        HandleAsync_Throws_ForbidException_When_User_Is_Not_Author_Or_Admin()
    {
        var command = new AddStep(Guid.NewGuid(), "Step");
    
        var recipe = new Domain.Entities.Recipe(Guid.NewGuid(), command.RecipeId, "Recipe", "Url", 30, DateTime.UtcNow);
        
        _repository.GetAsync(command.RecipeId).Returns(recipe);
        
        _authorization.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<object?>(),
            Arg.Any<IEnumerable<IAuthorizationRequirement>>()).Returns(AuthorizationResult.Failed());
    
        var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));
    
        exception.ShouldNotBeNull();
    
        exception.ShouldBeOfType<ForbidException>();
    
    }
    
    [Fact]
    public async Task
        HandleAsync_Calls_Repository_On_Success()
    {
        var command = new AddStep(Guid.NewGuid(), "Step");
    
        var recipe = new Domain.Entities.Recipe(Guid.NewGuid(), command.RecipeId, "Recipe", "Url", 30, DateTime.UtcNow);
        
        _repository.GetAsync(command.RecipeId).Returns(recipe);
        
        _authorization.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<object?>(),
            Arg.Any<IEnumerable<IAuthorizationRequirement>>()).Returns(AuthorizationResult.Success());
    
        var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));
    
        exception.ShouldBeNull();
    
        await _repository.Received(1).UpdateAsync(recipe);
    
    }
    #region ARRANGE
    private readonly IRecipeRepository _repository;
    private readonly IUserContextService _userContext;
    private readonly IAuthorizationService _authorization;
    private readonly ICommandHandler<AddStep> _commandHandler;
    public AddStepHandlerTests()
    {
        _repository = Substitute.For<IRecipeRepository>();
        _userContext = Substitute.For<IUserContextService>();
        _authorization = Substitute.For<IAuthorizationService>();

        _commandHandler = new AddStepHandler(_repository, _userContext, _authorization);
    }
    #endregion
}