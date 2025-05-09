namespace CuentasPorPagar.Dominio;

public interface ICommandRouter
{
    public Task InvokeAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class;

    public Task<TResult> InvokeAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class;
}