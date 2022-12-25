using CookingBook.Application.Commands.Review;
using CookingBook.Application.Exceptions;
using CookingBook.Application.Services;
using CookingBook.Domain;
using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Handlers.Review;

public class RemoveReviewHandler : ICommandHandler<RemoveReview>
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserContextService _userContext;

    public RemoveReviewHandler(IRecipeRepository recipeRepository, IUserRepository userRepository, IUserContextService userContext)
    {
        _recipeRepository = recipeRepository;
        _userRepository = userRepository;
        _userContext = userContext;
    }
    
    public async Task HandleAsync(RemoveReview command)
    {
        var recipe = await _recipeRepository.GetAsync(command.RecipeId);

        if (recipe is null)
        {
            throw new RecipeNotFoundException(command.RecipeId);
        }

        var review = recipe.GetReview(command.Name);

        if ((Guid)review.UserId != _userContext.GetUserId)
        {
            throw new ForbidException();
        }
        
        
        recipe.RemoveReview(command.Name);

        var recipeAuthor = await _userRepository.GetAsync(recipe.UserId);
        
        
        recipeAuthor.AddRate((short)-review.Rate);

        await _recipeRepository.UpdateAsync(recipe);
        await _userRepository.UpdateAsync(recipeAuthor);

    }
}