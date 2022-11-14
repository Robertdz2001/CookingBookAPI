using System.Text;
using CookingBook.Application.Services;
using CookingBook.Infrastructure.EF;
using CookingBook.Infrastructure.EF.Models;
using CookingBook.Infrastructure.EF.Options;
using CookingBook.Infrastructure.Jwt;
using CookingBook.Infrastructure.Services;
using CookingBook.Shared.Queries;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CookingBook.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPostgres(configuration);
        services.AddScoped<Seeder>();
        services.AddScoped<IPasswordHasher<UserReadModel>, PasswordHasher<UserReadModel>>();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddQueries();
        services.AddJwt(configuration);
        
        
        

        return services;
    }
}