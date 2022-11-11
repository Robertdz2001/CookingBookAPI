using CookingBook.Application.Exceptions;
using CookingBook.Domain;
using CookingBook.Domain.ValueObjects;
using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Handlers;

public class ChangeIngredientHandler : ICommandHandler<ChangeIngredient>
{
    private readonly IRecipeRepository _repository;

    public ChangeIngredientHandler(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(ChangeIngredient command)
    {
        var recipe = await _repository.GetAsync(command.RecipeId);

        if (recipe is null)
        {
            throw new RecipeNotFoundException(command.RecipeId);
        }

        var ingredient = new Ingredient(command.Name, command.Grams, command.CaloriesPerHundredGrams);
        
        recipe.ChangeIngredient(ingredient,command.IngredientToChangeName);

        await _repository.UpdateAsync(recipe);
    }
}