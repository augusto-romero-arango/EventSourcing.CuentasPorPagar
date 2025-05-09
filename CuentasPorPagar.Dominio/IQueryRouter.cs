namespace CuentasPorPagar.Dominio;

public interface IQueryRouter
{
    public Task<TResult> ResolveAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : class;
}