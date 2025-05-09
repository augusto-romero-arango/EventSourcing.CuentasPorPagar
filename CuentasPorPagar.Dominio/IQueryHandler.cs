namespace CuentasPorPagar.Dominio;

public interface IQueryHandler<TQuery, TResult>
{
    /// <summary>
    ///     Procesa la consulta de tipo <typeparamref name="TQuery" /> de manera asíncrona y retorna un resultado de tipo
    ///     <typeparamref name="TResult" />.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}