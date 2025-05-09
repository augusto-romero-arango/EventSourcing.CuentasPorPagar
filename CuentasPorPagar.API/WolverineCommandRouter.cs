using CuentasPorPagar.Dominio;
using Wolverine;

namespace CuentasPorPagar.API;

public class WolverineCommandRouter(IMessageBus messageBus) : ICommandRouter
{
    public Task InvokeAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class
    {
        return messageBus.InvokeAsync(command, cancellationToken);
    }

    public Task<TResult> InvokeAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class
    {
        return messageBus.InvokeAsync<TResult>(command, cancellationToken);
    }
}