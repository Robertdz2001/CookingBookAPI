using CookingBook.Application.Commands.Step;
using CookingBook.Application.Exceptions;
using CookingBook.Domain;
using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Handlers.Step;

public class AddStepHandler : ICommandHandler<AddStep>
{
    private readonly IRecipeRepository _repository;

    public AddStepHandler(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(AddStep command)
    {
        var recipe = await _repository.GetAsync(command.RecipeId);

        if (recipe is null)
        {
            throw new RecipeNotFoundException(command.RecipeId);
        }

        var newStep = new Domain.ValueObjects.Step(command.Name);
        
        recipe.AddStep(newStep);

        await _repository.UpdateAsync(recipe);

    }
}