using CookingBook.Application.Commands.Handlers.Review;
using CookingBook.Application.Commands.Review;
using CookingBook.Application.Exceptions;
using CookingBook.Application.Services;
using CookingBook.Domain;
using CookingBook.Shared.Abstractions.Commands;
using NSubstitute;
using Shouldly;

namespace CookingBook.Application.Tests.Review;

public class AddReviewHandlerTests
{
    [Fact]
    public async Task HandleAsync_Calls_Both_Repositories_On_Success()
    {
        var recipe = new Domain.Entities.Recipe(Guid.NewGuid(), Guid.NewGuid(),
            "Recipe", "Url", 30, DateTime.UtcNow);

        var user = new Domain.Entities.User(recipe.UserId, "UserName", "Pass");
        
        _userContext.GetUserId.Returns(Guid.NewGuid());

        _recipeRepository.GetAsync(recipe.Id).Returns(recipe);

        _userRepository.GetAsync(recipe.UserId).Returns(user);

        var exception = await Record.ExceptionAsync(() =>
            _commandHandler.HandleAsync(new AddReview(recipe.Id, "Review", "Content", 4)));
        
        exception.ShouldBeNull();

        await _recipeRepository.Received(1).UpdateAsync(recipe);
        await _userRepository.Received(1).UpdateAsync(user);
    }
    
    [Fact]
    public async Task HandleAsync_Throws_RecipeNotFoundException_When_There_Is_No_Recipe_With_Given_Id()
    {
        var id = Guid.NewGuid();

        _recipeRepository.GetAsync(id).Returns(default(Domain.Entities.Recipe));
        
        var exception = await Record.ExceptionAsync(() =>
            _commandHandler.HandleAsync(new AddReview(id, "Review", "Content", 4)));
        
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<RecipeNotFoundException>();
    }
    
    #region ARRANGE

    private readonly IUserContextService _userContext;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICommandHandler<AddReview> _commandHandler;

    public AddReviewHandlerTests()
    {
        _userContext = Substitute.For<IUserContextService>();
        _recipeRepository = Substitute.For<IRecipeRepository>();
        _userRepository = Substitute.For<IUserRepository>();

        _commandHandler = new AddReviewHandler(_userContext, _recipeRepository, _userRepository);
    }
    
    #endregion
}