using CookingBook.Application.Services;
using CookingBook.Domain;
using CookingBook.Infrastructure.EF.Contexts;
using CookingBook.Infrastructure.EF.Options;
using CookingBook.Infrastructure.EF.Repositories;
using CookingBook.Infrastructure.EF.Services;
using CookingBook.Shared.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CookingBook.Infrastructure.EF;

public static class Extensions
{
    public static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRecipeRepository, PostgresRecipeRepository>();
        services.AddScoped<IRecipeReadService, PostgresRecipeReadService>();

        var options = configuration.GetOptions<PostgresOptions>("Postgres");

        services.AddDbContext<ReadDbContext>(ctx 
            => ctx.UseNpgsql(options.ConnectionString));

        services.AddDbContext<WriteDbContext>(ctx
            => ctx.UseNpgsql(options.ConnectionString));

        return services;
    }
}