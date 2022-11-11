using CookingBook.Domain.Factories;
using CookingBook.Shared.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace CookingBook.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        services.AddCommands();
        services.AddSingleton<IRecipeFactory, RecipeFactory>();


        return services;
    }
}