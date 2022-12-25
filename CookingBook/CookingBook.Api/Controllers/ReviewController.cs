using CookingBook.Api.Models;
using CookingBook.Application.Commands.Review;
using CookingBook.Shared.Abstractions.Commands;
using Microsoft.AspNetCore.Mvc;

namespace CookingBook.Api.Controllers;
[Route("api/recipes/{RecipeId:guid}/reviews")]
public class ReviewController : BaseController
{

    private readonly ICommandDispatcher _commandDispatcher;

    public ReviewController(ICommandDispatcher commandDispatcher)
    {
        _commandDispatcher = commandDispatcher;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromRoute] Guid RecipeId, [FromBody] AddReviewModel model)
    {
        var command = new AddReview(RecipeId, model.Name, model.Content, model.Rate);

        await _commandDispatcher.DispatchAsync(command);

        return Created($"api/recipes/{RecipeId}/reviews/{model.Name}", null);
    }
    
    [HttpPut("{name}")]
    public async Task<IActionResult> Put([FromRoute] Guid RecipeId,[FromRoute] string name, [FromBody] AddReviewModel model)
    {
        var command = new ChangeReview(RecipeId, model.Name, model.Content, model.Rate,name);

        await _commandDispatcher.DispatchAsync(command);

        return Ok();
    }
    
    [HttpDelete("{name}")]
    public async Task<IActionResult> Put([FromRoute] Guid RecipeId,[FromRoute] string name)
    {
        var command = new RemoveReview(RecipeId, name);

        await _commandDispatcher.DispatchAsync(command);

        return NoContent();
    }
    
}