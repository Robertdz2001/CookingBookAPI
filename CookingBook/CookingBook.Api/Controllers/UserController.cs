using CookingBook.Api.Models;
using CookingBook.Application.Commands.User;
using CookingBook.Application.DTO;
using CookingBook.Application.Queries;
using CookingBook.Domain.Entities;
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

    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto dto)
    {
        var command = new RegisterUser(Guid.NewGuid(), dto.UserName, dto.Password,dto.ImageUrl, dto.ConfirmPassword);
        
        await _commandDispatcher.DispatchAsync(command);
        
        return Ok();
    }
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await _queryDispatcher.DispatchAsync(new GetJwtToken{Dto = dto});


        return Ok(token);
    }

    [HttpGet]

    public async Task<ActionResult<UserDto>> GetUser()
    {
        var user = await _queryDispatcher.DispatchAsync(new GetUser());

        return Ok(user);
    }
    

    [Authorize(Roles="Admin")]
    [HttpPut("{id:guid}/role")]
    public async Task<ActionResult> ChangeRole([FromRoute] Guid id,[FromBody] int roleId)
    {
        await _commandDispatcher.DispatchAsync(new ChangeUserRole(id, roleId));

        return NoContent();
    }
}