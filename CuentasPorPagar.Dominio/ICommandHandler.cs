namespace CuentasPorPagar.Dominio;

/// <summary>
/// Define un manejador para procesar comandos de tipo <typeparamref name="TCommand"/> asíncrono.
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public interface ICommandHandlerAsync<TCommand>
{
    /// <summary>
    /// Procesa el comando de tipo <typeparamref name="TCommand"/> de manera asíncrona.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}

/// <summary>
/// Define un manejador para procesar comandos de tipo <typeparamref name="TCommand"/> síncrono.
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public interface ICommandHandler<TCommand>
{
    /// <summary>
    /// Procesa el comando de tipo <typeparamref name="TCommand"/> de manera síncrona.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    void Handle(TCommand command);
}

/// <summary>
/// Define un manejador para procesar comandos de tipo <typeparamref name="TCommand"/> donde el handler retorna <typeparamref name="TResult"/> asíncrono.
/// </summary>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface ICommandHandlerAsync<TCommand, TResult>
{
    /// <summary>
    /// Procesa el comando de tipo <typeparamref name="TCommand"/> de manera asíncrona y retorna un resultado de tipo <typeparamref name="TResult"/>.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}

/// <summary>
/// Define un manejador para procesar comandos de tipo <typeparamref name="TCommand"/> donde el handler retorna <typeparamref name="TResult"/> síncrono.
/// </summary>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface ICommandHandler<TCommand, TResult>
{
    /// <summary>
    /// Procesa el comando de tipo <typeparamref name="TCommand"/> de manera síncrona y retorna un resultado de tipo <typeparamref name="TResult"/>.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    TResult Handle(TCommand command);
}