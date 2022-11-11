using CookingBook.Application.Commands.Tool;
using CookingBook.Application.Exceptions;
using CookingBook.Domain;
using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Handlers.Tool;

public class ChangeToolHandler: ICommandHandler<ChangeTool>
{
    private readonly IRecipeRepository _repository;

    public ChangeToolHandler(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(ChangeTool command)
    {
        var recipe = await _repository.GetAsync(command.RecipeId);

        if (recipe is null)
        {
            throw new RecipeNotFoundException(command.RecipeId);
        }

        var tool = new Domain.ValueObjects.Tool(command.Name, command.Quantity);
        
        recipe.ChangeTool(tool,command.ToolToChangeName);

        await _repository.UpdateAsync(recipe);
    }
}