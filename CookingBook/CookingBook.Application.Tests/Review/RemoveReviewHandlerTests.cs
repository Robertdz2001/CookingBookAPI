using CookingBook.Application.Commands.Handlers.Review;
using CookingBook.Application.Commands.Review;
using CookingBook.Application.Exceptions;
using CookingBook.Application.Services;
using CookingBook.Domain;
using CookingBook.Shared.Abstractions.Commands;
using NSubstitute;
using Shouldly;

namespace CookingBook.Application.Tests.Review;

public class RemoveReviewHandlerTests
{

    [Fact]
    public async Task
        HandleAsync_Calls_Both_Repositories_On_Success()
    {
        var recipe = new Domain.Entities.Recipe(Guid.NewGuid(), Guid.NewGuid(),
            "Recipe", "Url", 30, DateTime.UtcNow);

        var uid = Guid.NewGuid();

        var review = new Domain.ValueObjects.Review("Review", "Content",3 ,uid);
        
        recipe.AddReview(review);

        var user = new Domain.Entities.User(uid, "User", "Url","Pass");
        
        _recipeRepository.GetAsync(recipe.Id).Returns(recipe);

        _userContext.GetUserId.Returns(uid);

        _userRepository.GetAsync(recipe.UserId).Returns(user);

        var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(new RemoveReview(recipe.Id, "Review")));
        
        exception.ShouldBeNull();

        await _recipeRepository.Received(1).UpdateAsync(recipe);
        
        await _userRepository.Received(1).UpdateAsync(user);


    }
    
    [Fact]
    public async Task
        HandleAsync_Throws_RecipeNotFoundException_When_There_Is_No_Recipe_With_Given_Id()
    {

        var id = Guid.NewGuid();

        _recipeRepository.GetAsync(id).Returns(default(Domain.Entities.Recipe));
        
        var exception = await Record.ExceptionAsync(() =>
            _commandHandler.HandleAsync(new RemoveReview(id, "Review")));
        
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<RecipeNotFoundException>();

    }

    [Fact]
    public async Task
        HandleAsync_Throws_ForbidException_When_User_Is_Not_Review_Author()
    {
        var recipe = new Domain.Entities.Recipe(Guid.NewGuid(), Guid.NewGuid(),
            "Recipe", "Url", 30, DateTime.UtcNow);

        var uid = Guid.NewGuid();

        var review = new Domain.ValueObjects.Review("Review", "Content",3 ,Guid.NewGuid());
        
        recipe.AddReview(review);

        var user = new Domain.Entities.User(uid, "User","Url", "Pass");
        
        _recipeRepository.GetAsync(recipe.Id).Returns(recipe);

        _userContext.GetUserId.Returns(uid);

        _userRepository.GetAsync(recipe.UserId).Returns(user);

        var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(new RemoveReview(recipe.Id, "Review")));
        
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ForbidException>();
    }

    
    #region ARRANGE

    private readonly IUserRepository _userRepository;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IUserContextService _userContext;
    private readonly ICommandHandler<RemoveReview> _commandHandler;

    public RemoveReviewHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _recipeRepository = Substitute.For<IRecipeRepository>();
        _userContext = Substitute.For<IUserContextService>();

        _commandHandler = new RemoveReviewHandler(_recipeRepository, _userRepository, _userContext);
    }
    #endregion
}