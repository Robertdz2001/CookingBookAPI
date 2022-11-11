using CookingBook.Application.Commands.Tool;
using CookingBook.Application.Exceptions;
using CookingBook.Domain;
using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Handlers.Tool;

public class AddToolHandler : ICommandHandler<AddTool>
{
    private readonly IRecipeRepository _repository;

    public AddToolHandler(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(AddTool command)
    {
        var recipe = await _repository.GetAsync(command.RecipeId);

        if (recipe is null)
        {
            throw new RecipeNotFoundException(command.RecipeId);
        }

        var newTool = new Domain.ValueObjects.Tool(command.Name, command.Quantity);
        
        recipe.AddTool(newTool);

        await _repository.UpdateAsync(recipe);

    }
}