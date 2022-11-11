using CookingBook.Api.Models;
using CookingBook.Application.Commands.Tool;
using CookingBook.Shared.Abstractions.Commands;
using CookingBook.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CookingBook.Api.Controllers;

[Route("api/recipes/{RecipeId:guid}/tools")]
public class ToolController : BaseController
{

    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public ToolController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }
    [HttpPost]

    public async Task<IActionResult> Put([FromRoute] Guid RecipeId, [FromBody] AddToolModel model)
    {
        var command = new AddTool(RecipeId, model.Name, model.Quantity);
        
        await _commandDispatcher.DispatchAsync(command);

        return Ok();
    }
    
    [HttpDelete("{name}")]

    public async Task<IActionResult> DeleteTool([FromRoute] Guid RecipeId,[FromRoute] string name)
    {
        var command = new RemoveTool(RecipeId,name);
        
        await _commandDispatcher.DispatchAsync(command);

        return Ok();
    }
    
    [HttpPut("{name}")]

    public async Task<IActionResult> PutTool([FromRoute] Guid RecipeId,[FromRoute] string name, [FromBody] AddToolModel model)
    {
        var command = new ChangeTool(RecipeId, name,model.Name,model.Quantity);
        
        await _commandDispatcher.DispatchAsync(command);

        return Ok();
    }
}