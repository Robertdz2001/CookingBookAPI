using CookingBook.Application.Commands.Review;
using CookingBook.Application.Exceptions;
using CookingBook.Application.Services;
using CookingBook.Domain;
using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Handlers.Review;

public class AddReviewHandler : ICommandHandler<AddReview>
{
    private readonly IUserContextService _userContext;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IUserRepository _userRepository;
    
    public AddReviewHandler(IUserContextService userContext, IRecipeRepository recipeRepository, IUserRepository userRepository)
    {
        _userContext = userContext;
        _recipeRepository = recipeRepository;
        _userRepository = userRepository;
    }
    
    public async Task HandleAsync(AddReview command)
    {
        var recipe = await _recipeRepository.GetAsync(command.RecipeId);
        
        if (recipe is null)
        {
            throw new RecipeNotFoundException(command.RecipeId);
        }

        var review =
            new Domain.ValueObjects.Review(command.Name, command.Content, command.Rate, (Guid)_userContext.GetUserId);
        
        recipe.AddReview(review);

        var recipeAuthor = await _userRepository.GetAsync(recipe.UserId);
        
        recipeAuthor.AddRate(review.Rate);

        await _recipeRepository.UpdateAsync(recipe);
        await _userRepository.UpdateAsync(recipeAuthor);

    }
}