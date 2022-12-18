using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CookingBook.Shared.Services;

public class AppInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public AppInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var dbContextTypes = AppDomain.CurrentDomain.GetAssemblies() //gives us our 2 DbContexts
            .SelectMany(a => a.GetTypes())
            .Where(a=>typeof(DbContext)
                .IsAssignableFrom(a) && !a.IsInterface && a != typeof(DbContext));

        using var scope = _serviceProvider.CreateScope();

        foreach(var dbContextType in dbContextTypes)
        {
            var dbContext = scope.ServiceProvider.GetRequiredService(dbContextType) as DbContext;

            if(dbContext is null)
            {
                continue;
            }

            if (dbContext.Database.IsRelational())
            {
                await dbContext.Database.MigrateAsync(cancellationToken);
            }
        }

            


    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}