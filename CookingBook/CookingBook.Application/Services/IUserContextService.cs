using System.Security.Claims;
using CookingBook.Domain.Consts;

namespace CookingBook.Application.Services;

public interface IUserContextService
{
    ClaimsPrincipal User { get; }
    Guid? GetUserId { get; }
    
    Role? GetUserRole { get; }
}