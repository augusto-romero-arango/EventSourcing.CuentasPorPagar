using FluentAssertions;
using Xunit.Sdk;

namespace CuentasPorPagar.Dominio.Tests.Tests;

public class TestHandlers : CommandHandlerTest<ActualizarLista>
{
    protected override ICommandHandler<ActualizarLista> Handler => new ActualizarListaHandler(eventStore);

    [Fact]
    public void Debe_CuandoAciertoUnaLista_ValidarEnOrden()
    {
        When(new ActualizarLista(_aggregateId));

        Then(new ListaActualizada(new List<string> {"1", "2"}));

        And<ListaAggregateRoot>((entidad) => entidad.Lista, new List<string> {"1", "2"});

        var caller = () => And<ListaAggregateRoot>((entidad) => entidad.Lista, new List<string> {"2", "1"},
            options => options.WithStrictOrdering());

        caller.Should()
            .Throw<XunitException>();
    }
}

public record ActualizarLista(Guid Id);

public record ListaActualizada(List<string> List);

public class ActualizarListaHandler(IEventStore eventStore) : ICommandHandler<ActualizarLista>
{
    public void Handle(ActualizarLista command)
    {
        eventStore.AppendEvent(command.Id, new ListaActualizada(new List<string> {"1", "2"}));
    }
}

public class ListaAggregateRoot : AggregateRoot
{
    public void Apply(ListaActualizada @event)
    {
        Lista = @event.List;
    }

    public List<string> Lista { get; private set; } = new List<string>();
}