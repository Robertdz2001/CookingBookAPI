using CookingBook.Application.Commands.Review;
using CookingBook.Application.Exceptions;
using CookingBook.Application.Services;
using CookingBook.Domain;
using CookingBook.Domain.Entities;
using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Handlers.Review;

public class ChangeReviewHandler : ICommandHandler<ChangeReview>
{
  
    private readonly IRecipeRepository _recipeRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserContextService _userContext;
    
    public ChangeReviewHandler(IRecipeRepository recipeRepository, IUserRepository userRepository, IUserContextService userContext)
    {
        _recipeRepository = recipeRepository;
        _userRepository = userRepository;
        _userContext = userContext;
    }

    public async Task HandleAsync(ChangeReview command)
    {
        var recipe = await _recipeRepository.GetAsync(command.RecipeId);

        if (recipe is null)
        {
            throw new RecipeNotFoundException(command.RecipeId);
        }

        var reviewToChange = recipe.GetReview(command.ReviewToChange);

        if ((Guid)reviewToChange.UserId != _userContext.GetUserId && _userContext.GetUserRole!="Admin")
        {
            throw new ForbidException();
        }

        var recipeAuthor = await _userRepository.GetAsync(recipe.UserId);

        var review = new Domain.ValueObjects.Review(command.Name, command.Content, command.Rate, reviewToChange.UserId);
        
        recipe.ChangeReview(review,command.ReviewToChange);

        var newRate = (short)(review.Rate - reviewToChange.Rate);
        
        recipeAuthor.AddRate(newRate);


        await _recipeRepository.UpdateAsync(recipe);
        await _userRepository.UpdateAsync(recipeAuthor);


    }
}