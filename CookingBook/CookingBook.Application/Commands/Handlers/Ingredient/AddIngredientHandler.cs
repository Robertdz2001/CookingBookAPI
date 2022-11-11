using CookingBook.Application.Exceptions;
using CookingBook.Domain;
using CookingBook.Domain.ValueObjects;
using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Handlers;

public class AddIngredientHandler : ICommandHandler<AddIngredient>
{
    private readonly IRecipeRepository _repository;

    public AddIngredientHandler(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(AddIngredient command)
    {
        var recipe = await _repository.GetAsync(command.RecipeId);

        if (recipe is null)
        {
            throw new RecipeNotFoundException(command.RecipeId);
        }

        var newIngredient = new Ingredient(command.Name, command.Grams, command.CaloriesPerHundredGrams);
        
        recipe.AddIngredient(newIngredient);
        
        await _repository.UpdateAsync(recipe);
    }
}