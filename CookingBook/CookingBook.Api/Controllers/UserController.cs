using CookingBook.Api.Authorize;
using CookingBook.Application.Commands.User;
using CookingBook.Application.Queries;
using CookingBook.Domain.Consts;
using CookingBook.Infrastructure.Jwt.DTO;
using CookingBook.Shared.Abstractions.Commands;
using CookingBook.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookingBook.Api.Controllers;
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public UserController(ICommandDispatcher commandDispatcher,IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    [HttpPost("register")]

    public async Task<IActionResult> RegisterUser([FromBody] RegisterUser command)
    {
        await _commandDispatcher.DispatchAsync(command);
        
        return Ok();
    }
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await _queryDispatcher.DispatchAsync(new GetJwtToken{Dto = dto});


        return Ok(token);
    }
    [Authorize]
    [HttpGet("recipes")]
    public async Task<ActionResult> Get()
    {
        var result = await _queryDispatcher.DispatchAsync(new GetUsersRecipes());


        return Ok(result);
    }

    [AuthorizeEnum(Role.Admin)]
    [HttpPut("{id:guid}/role")]
    public async Task<ActionResult> ChangeRole([FromRoute] Guid id,[FromBody] Role role)
    {
        await _commandDispatcher.DispatchAsync(new ChangeUserRole(id, role));

        return NoContent();
    }
}