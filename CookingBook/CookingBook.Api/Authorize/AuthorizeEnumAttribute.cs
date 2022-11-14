using CookingBook.Domain.Consts;
using Microsoft.AspNetCore.Authorization;

namespace CookingBook.Api.Authorize;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
public class AuthorizeEnumAttribute : AuthorizeAttribute
{
    public AuthorizeEnumAttribute(params object[] roles)
    {
        if (roles.Any(r => r.GetType().BaseType != typeof(Enum)))
            throw new ArgumentException("roles");

        this.Roles = string.Join(",", roles.Select(r => Enum.GetName(r.GetType(), r)));
    }
}