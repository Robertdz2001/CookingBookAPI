using CookingBook.Application.Exceptions;
using CookingBook.Domain;
using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Handlers.Recipe;

public class RemoveRecipeHandler : ICommandHandler<RemoveRecipe>
{
    private readonly IRecipeRepository _repository;


    public RemoveRecipeHandler(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(RemoveRecipe command)
    {
        var recipeToRemove = await _repository.GetAsync(command.Id);

        if (recipeToRemove is null)
        {
            throw new RecipeNotFoundException(command.Id);
        }

        await _repository.DeleteAsync(recipeToRemove);

    }
}