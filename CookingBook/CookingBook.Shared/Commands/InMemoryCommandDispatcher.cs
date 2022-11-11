using CookingBook.Shared.Abstractions.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace CookingBook.Shared.Commands;

public sealed class InMemoryCommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public InMemoryCommandDispatcher(IServiceProvider serviceProvider)
        =>  _serviceProvider = serviceProvider;
    


    public async Task DispatchAsync<TCommand>(TCommand command) where TCommand : class, ICommand
    {
        using var scope = _serviceProvider.CreateScope();

        var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();

        await handler.HandleAsync(command);
    }
}