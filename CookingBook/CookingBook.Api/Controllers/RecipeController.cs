using CookingBook.Api.Models;
using CookingBook.Application.Commands;
using CookingBook.Application.Commands.Handlers;
using CookingBook.Application.Commands.Step;
using CookingBook.Application.Commands.Tool;
using CookingBook.Application.DTO;
using CookingBook.Application.Queries;
using CookingBook.Shared.Abstractions.Commands;
using CookingBook.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookingBook.Api.Controllers;

[Route("api/recipes")]
public class RecipeController : BaseController
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;
    public RecipeController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RecipeDto>>> Get([FromQuery] SearchRecipes query)
    {
        var result = await _queryDispatcher.DispatchAsync(query);

        return Ok(result);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RecipeDto>> Get([FromRoute] Guid id)
    {
        var query = new GetRecipe { Id = id };
        
        var result = await _queryDispatcher.DispatchAsync(query);

        return OkOrNotFound(result);
    }
    
    
    [HttpGet("user")]
    public async Task<ActionResult> Get()
    {
        var result = await _queryDispatcher.DispatchAsync(new GetUsersRecipes());


        return Ok(result);
    }

    [HttpDelete("{id:guid}")]

    public async Task<IActionResult> DeleteRecipe([FromRoute] Guid id)
    {
        var command = new RemoveRecipe(id);

        await _commandDispatcher.DispatchAsync(command);
        
        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> PostRecipe([FromBody] CreateRecipeDto dto)
    {
        var command = new CreateRecipe(Guid.NewGuid(), dto.Name, dto.ImageUrl, dto.PrepTime);
        
        await _commandDispatcher.DispatchAsync(command);
        
        return CreatedAtAction(nameof(Get), new {id=command.Id},null);
    }
    
    
  
    
    
    

    
}