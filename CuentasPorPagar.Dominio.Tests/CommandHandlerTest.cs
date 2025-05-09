using FluentAssertions;

namespace CuentasPorPagar.Dominio.Tests;

public abstract class CommandHandlerTest<TCommand> : CommandHandlerTestBase
{
    /// <summary>
    /// The command handler, to be provided in the Test class.
    /// This to account for additional injections
    /// </summary>
    protected abstract ICommandHandler<TCommand> Handler { get; }

    /// <summary>
    /// Triggers the handling of a command against the configured events.
    /// </summary>
    protected void When(TCommand command)
    {
        Handler.Handle(command);
    }
}

public abstract class CommandHandlerTest<TCommand, TResult> : CommandHandlerTestBase
{
    /// <summary>
    /// The command handler, to be provided in the Test class.
    /// This to account for additional injections
    /// </summary>
    protected abstract ICommandHandler<TCommand, TResult> Handler { get; }

    /// <summary>
    /// Triggers the handling of a command against the configured events.
    /// </summary>
    protected TResult When(TCommand command)
    {
        return Handler.Handle(command);
    }
}