using CookingBook.Shared.Abstractions.Exceptions;
using Microsoft.AspNetCore.Http;

namespace CookingBook.Shared.Exceptions;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (CookingBookBadRequestException e)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(e.Message);
        }
        catch (CookingBookNotFoundException e)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(e.Message);
        }
        catch (CookingBookException e)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync(e.Message);
        }
        
        catch (Exception e)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Something went wrong.");
        }
        
    }
}
