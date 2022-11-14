using CookingBook.Application.Authorization;
using CookingBook.Application.Exceptions;
using CookingBook.Application.Services;
using CookingBook.Domain;
using CookingBook.Shared.Abstractions.Commands;
using Microsoft.AspNetCore.Authorization;

namespace CookingBook.Application.Commands.Handlers.Recipe;

public class RemoveRecipeHandler : ICommandHandler<RemoveRecipe>
{
    private readonly IRecipeRepository _repository;
    private readonly IUserContextService _userContext;
    private readonly IAuthorizationService _authorization;

    public RemoveRecipeHandler(IRecipeRepository repository, IAuthorizationService authorization, IUserContextService userContext)
    {
        _repository = repository;
        _authorization = authorization;
        _userContext = userContext;
    }

    public async Task HandleAsync(RemoveRecipe command)
    {
        var recipeToRemove = await _repository.GetAsync(command.Id);

        if (recipeToRemove is null)
        {
            throw new RecipeNotFoundException(command.Id);
        }

        var authorizationResult =  await _authorization
          .AuthorizeAsync(_userContext.User, recipeToRemove,new ResourceOperationRequirement(ResourceOperation.Delete));

        if (!authorizationResult.Succeeded)
        {
            throw new ForbidException();
        }

        await _repository.DeleteAsync(recipeToRemove);

    }
}