using System.Security.Claims;
using CookingBook.Domain.Entities;

namespace CookingBook.Application.Services;

public interface IUserContextService
{
    ClaimsPrincipal User { get; }
    Guid? GetUserId { get; }
    
    string? GetUserRole { get; }
}