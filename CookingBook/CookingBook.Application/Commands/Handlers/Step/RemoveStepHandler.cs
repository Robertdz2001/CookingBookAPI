using CookingBook.Application.Commands.Step;
using CookingBook.Application.Exceptions;
using CookingBook.Domain;
using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Handlers.Step;

public class RemoveStepHandler: ICommandHandler<RemoveStep>
{
    private readonly IRecipeRepository _repository;

    public RemoveStepHandler(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(RemoveStep command)
    {
        var recipe = await _repository.GetAsync(command.RecipeId);

        if (recipe is null)
        {
            throw new RecipeNotFoundException(command.RecipeId);
        }
        
        recipe.RemoveStep(command.Name);
        
        await _repository.UpdateAsync(recipe);
    }
}