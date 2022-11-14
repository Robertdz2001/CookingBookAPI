using CookingBook.Application.Authorization;
using CookingBook.Application.Commands.Tool;
using CookingBook.Application.Exceptions;
using CookingBook.Application.Services;
using CookingBook.Domain;
using CookingBook.Shared.Abstractions.Commands;
using Microsoft.AspNetCore.Authorization;

namespace CookingBook.Application.Commands.Handlers.Tool;

public class ChangeToolHandler: ICommandHandler<ChangeTool>
{
    private readonly IRecipeRepository _repository;
    private readonly IUserContextService _userContext;
    private readonly IAuthorizationService _authorization;
    
    public ChangeToolHandler(IRecipeRepository repository, IUserContextService userContext, IAuthorizationService authorization)
    {
        _repository = repository;
        _userContext = userContext;
        _authorization = authorization;
    }

    public async Task HandleAsync(ChangeTool command)
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

        var tool = new Domain.ValueObjects.Tool(command.Name, command.Quantity);
        
        recipe.ChangeTool(tool,command.ToolToChangeName);

        await _repository.UpdateAsync(recipe);
    }
}