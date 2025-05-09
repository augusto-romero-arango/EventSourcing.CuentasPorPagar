using FluentAssertions;

namespace CuentasPorPagar.Dominio.Tests;

public abstract class CommandHandlerAsyncTest<TCommand> : CommandHandlerTestBase
{
    /// <summary>
    /// The command handler, to be provided in the Test class.
    /// This to account for additional injections
    /// </summary>
    protected abstract ICommandHandlerAsync<TCommand> Handler { get; }

    /// <summary>
    /// Triggers the handling of a command against the configured events.
    /// </summary>
    protected Task WhenAsync(TCommand command)
    {
        return Handler.HandleAsync(command);
    }
}

public abstract class CommandHandlerAsyncTest<TCommand, TResult> : CommandHandlerTestBase
{
    /// <summary>
    /// The command handler, to be provided in the Test class.
    /// This to account for additional injections
    /// </summary>
    protected abstract ICommandHandlerAsync<TCommand, TResult> Handler { get; }

    /// <summary>
    /// Triggers the handling of a command against the configured events.
    /// </summary>
    protected Task<TResult> WhenAsync(TCommand command)
    {
        return Handler.HandleAsync(command);
    }
}