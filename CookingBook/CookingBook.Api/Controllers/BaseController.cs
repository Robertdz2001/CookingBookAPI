using Microsoft.AspNetCore.Mvc;

namespace CookingBook.Api.Controllers;
[ApiController]
public class BaseController : ControllerBase
{
    protected ActionResult<TResult> OkOrNotFound<TResult>(TResult result)
        => result is null ? NotFound() : Ok(result); 
}