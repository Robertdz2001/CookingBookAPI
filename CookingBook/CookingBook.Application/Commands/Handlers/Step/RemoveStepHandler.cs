using CookingBook.Application.Authorization;
using CookingBook.Application.Commands.Step;
using CookingBook.Application.Exceptions;
using CookingBook.Application.Services;
using CookingBook.Domain;
using CookingBook.Shared.Abstractions.Commands;
using Microsoft.AspNetCore.Authorization;

namespace CookingBook.Application.Commands.Handlers.Step;

public class RemoveStepHandler: ICommandHandler<RemoveStep>
{
    private readonly IRecipeRepository _repository;
    private readonly IUserContextService _userContext;
    private readonly IAuthorizationService _authorization;
    public RemoveStepHandler(IRecipeRepository repository, IAuthorizationService authorization, IUserContextService userContext)
    {
        _repository = repository;
        _authorization = authorization;
        _userContext = userContext;
    }

    public async Task HandleAsync(RemoveStep command)
    {
        var recipe = await _repository.GetAsync(command.RecipeId);

        if (recipe is null)
        {
            throw new RecipeNotFoundException(command.RecipeId);
        }
        
        var authorizationResult =  await _authorization
            .AuthorizeAsync(_userContext.User, recipe,new ResourceOperationRequirement(ResourceOperation.Update));

        if (!authorizationResult.Succeeded)
        {
            throw new ForbidException();
        }
        
        recipe.RemoveStep(command.Name);
        
        await _repository.UpdateAsync(recipe);
    }
}