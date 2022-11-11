using CookingBook.Api.Models;
using CookingBook.Application.Commands.Step;
using CookingBook.Shared.Abstractions.Commands;
using CookingBook.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CookingBook.Api.Controllers;

[Route("api/recipes/{RecipeId:guid}/steps")]
public class StepController : BaseController
{

    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public StepController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }
    
      
    [HttpPost]

    public async Task<IActionResult> PostStep([FromRoute] Guid RecipeId, [FromBody] AddStepModel model)
    {
        var command = new AddStep(RecipeId, model.Name);
        
        await _commandDispatcher.DispatchAsync(command);

        return Ok();
    }
    
    [HttpDelete("{name}")]

    public async Task<IActionResult> DeleteStep([FromRoute] Guid RecipeId,[FromRoute] string name)
    {
        var command = new RemoveStep(RecipeId,name);
        
        await _commandDispatcher.DispatchAsync(command);

        return Ok();
    }
    [HttpPut("{name}")]

    public async Task<IActionResult> PutStep([FromRoute] Guid RecipeId,[FromRoute] string name, [FromBody] AddStepModel model)
    {
        var command = new ChangeStep(RecipeId, name,model.Name);
        
        await _commandDispatcher.DispatchAsync(command);

        return Ok();
    }
}