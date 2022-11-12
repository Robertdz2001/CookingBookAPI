using System.Security.Claims;
using CookingBook.Application.Services;
using CookingBook.Domain.Consts;
using Microsoft.AspNetCore.Http;

namespace CookingBook.Infrastructure.Services;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;


    public Guid? GetUserId =>
        User is null ? null : (Guid?)Guid.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
    
    public Role? GetUserRole =>
        User is null ? null :(Role?) Enum.Parse(typeof(Role),User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
}