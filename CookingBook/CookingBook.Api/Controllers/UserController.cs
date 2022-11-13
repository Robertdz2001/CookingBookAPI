using CookingBook.Application.Commands.User;
using CookingBook.Application.Queries;
using CookingBook.Infrastructure.Jwt.DTO;
using CookingBook.Infrastructure.Jwt.Services;
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
    private readonly IGenerateJwtToken _jwtService;

    public UserController(ICommandDispatcher commandDispatcher, IGenerateJwtToken jwtService, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _jwtService = jwtService;
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
        var userCredentials = await _jwtService.Generate(dto);


        return Ok(userCredentials);
    }
    [Authorize]
    [HttpGet("recipes")]
    public async Task<ActionResult> Get()
    {
        var result = await _queryDispatcher.DispatchAsync(new GetUsersRecipes());


        return Ok(result);
    }
    
}