using CookingBook.Api.Models;
using CookingBook.Application.Commands;
using CookingBook.Application.Commands.Handlers;
using CookingBook.Shared.Abstractions.Commands;
using CookingBook.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CookingBook.Api.Controllers;

[Route("api/recipes/{RecipeId:guid}/ingredients")]
public class IngredientController : BaseController
{
    
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;
    public IngredientController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }
    
    [HttpPost]

    public async Task<IActionResult> PostIngredient([FromRoute] Guid RecipeId, [FromBody] AddIngredientModel model)
    {
        var command = new AddIngredient(RecipeId, model.Name, model.Grams, model.CaloriesPerHundredGrams);
        
        await _commandDispatcher.DispatchAsync(command);

        return Ok();
    }
    [HttpDelete("{name}")]

    public async Task<IActionResult> DeleteIngredient([FromRoute] Guid RecipeId,[FromRoute] string name)
    {
        var command = new RemoveIngredient(RecipeId,name);
        
        await _commandDispatcher.DispatchAsync(command);

        return Ok();
    }
    
    [HttpPut("{name}")]

    public async Task<IActionResult> PutIngredient([FromRoute] Guid RecipeId,[FromRoute] string name, [FromBody] AddIngredientModel model)
    {
        var command = new ChangeIngredient(RecipeId, name,model.Name, model.Grams, model.CaloriesPerHundredGrams);
        
        await _commandDispatcher.DispatchAsync(command);

        return Ok();
    }

}