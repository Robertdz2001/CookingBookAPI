using CookingBook.Application.Commands.Tool;
using CookingBook.Application.Exceptions;
using CookingBook.Domain;
using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Handlers.Tool;

public class RemoveToolHandler: ICommandHandler<RemoveTool>
{
    private readonly IRecipeRepository _repository;

    public RemoveToolHandler(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(RemoveTool command)
    {
        var recipe = await _repository.GetAsync(command.RecipeId);

        if (recipe is null)
        {
            throw new RecipeNotFoundException(command.RecipeId);
        }
        
        recipe.RemoveTool(command.Name);
        
        await _repository.UpdateAsync(recipe);
    }
}