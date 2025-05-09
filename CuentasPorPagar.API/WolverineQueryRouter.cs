using CuentasPorPagar.Dominio;
using Wolverine;

namespace CuentasPorPagar.API;

public class WolverineQueryRouter(IMessageBus messageBus) : IQueryRouter
{
    public Task<TResult> ResolveAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : class
    {
        return messageBus.InvokeAsync<TResult>(query, cancellationToken);
    }
}