using CookingBook.Application.Commands.Step;
using CookingBook.Application.Exceptions;
using CookingBook.Domain;
using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Handlers.Step;

public class ChangeStepHandler: ICommandHandler<ChangeStep>
{
    private readonly IRecipeRepository _repository;

    public ChangeStepHandler(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(ChangeStep command)
    {
        var recipe = await _repository.GetAsync(command.RecipeId);

        if (recipe is null)
        {
            throw new RecipeNotFoundException(command.RecipeId);
        }

        var step = new Domain.ValueObjects.Step(command.Name);
        
        recipe.ChangeStep(step,command.StepToChangeName);

        await _repository.UpdateAsync(recipe);
    }
}