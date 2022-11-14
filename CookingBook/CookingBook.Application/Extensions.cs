using CookingBook.Application.Authorization;
using CookingBook.Domain.Entities;
using CookingBook.Domain.Factories;
using CookingBook.Shared.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CookingBook.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        services.AddCommands();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
        services.AddSingleton<IRecipeFactory, RecipeFactory>();
        services.AddSingleton<IUserFactory, UserFactory>();

        return services;
    }
}