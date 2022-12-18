using System.Security.Claims;
using CookingBook.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace CookingBook.Application.Authorization;

public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement,Recipe>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement,
        Recipe recipe)
    {
        if (requirement.ResourceOperation is ResourceOperation.Create or ResourceOperation.Read)
        {
            context.Succeed(requirement);
        }

        var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var userRole = context.User.FindFirst(c => c.Type == ClaimTypes.Role).Value;
        
        if (recipe.UserId.Equals(Guid.Parse(userId)) || userRole =="Admin")
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }   
}