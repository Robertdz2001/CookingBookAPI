using CookingBook.Application.Exceptions;
using CookingBook.Domain;
using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Handlers;

public class RemoveIngredientHandler : ICommandHandler<RemoveIngredient>
{
    private readonly IRecipeRepository _repository;

    public RemoveIngredientHandler(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(RemoveIngredient command)
    {
        var recipe = await _repository.GetAsync(command.RecipeId);

        if (recipe is null)
        {
            throw new RecipeNotFoundException(command.RecipeId);
        }
        
        recipe.RemoveIngredient(command.Name);
        
        await _repository.UpdateAsync(recipe);
    }
}