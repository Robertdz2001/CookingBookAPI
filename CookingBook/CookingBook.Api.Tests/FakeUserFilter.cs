using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CookingBook.Api.Tests;

public class FakeUserFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var claimsPrincipal = new ClaimsPrincipal();
        
        claimsPrincipal.AddIdentity(new ClaimsIdentity(
            
            new []
            {
                new Claim(ClaimTypes.NameIdentifier,"bb21ce33-ea66-4c56-aefc-5f8588f95766"),
                new Claim(ClaimTypes.Role,"0")
            }));
        context.HttpContext.User = claimsPrincipal;

        await next();
    }
}