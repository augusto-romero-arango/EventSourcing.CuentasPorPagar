namespace CuentasPorPagar.Dominio.Tests.Tests;

public class ObjectTests : CommandHandlerTest<ActualizarObjeto>
{
    protected override ICommandHandler<ActualizarObjeto> Handler => new ActualizarObjetoHandler(eventStore);

    [Fact]
    public void Debe_CuandoAciertoUnObjeto_ValidarEnOrden()
    {
        var esperado = new ObjetoComplejo
        {
            Nombre = "Updated Name",
            FechaNacimiento = DateTime.Today,
            Lista = new List<string> {"New Item"},
            Monto = 100
        };

        When(new ActualizarObjeto(_aggregateId));

        Then(new ObjetoComplejoActualizado(new ObjetoComplejo
        {
            Nombre = "Updated Name",
            FechaNacimiento = esperado.FechaNacimiento,
            Lista = new List<string> {"New Item"},
            Monto = 100
        }));

        And<ObjetoComplejoAggregate>((entidad) => entidad.ObjetoComplejo, esperado);
    }
}

public record ActualizarObjeto(Guid Id);

public class ObjetoComplejoAggregate : AggregateRoot
{
    public void Apply(ObjetoComplejoActualizado @event)
    {
        ObjetoComplejo = @event.Objeto;
    }

    public ObjetoComplejo ObjetoComplejo { get; private set; } = null!;
}

public class ObjetoComplejo
{
    public string Nombre { get; set; } = null!;
    public DateTime FechaNacimiento { get; set; }
    public List<string> Lista { get; set; } = new List<string>();
    public decimal Monto { get; set; }
}

public record ObjetoComplejoActualizado(ObjetoComplejo Objeto);

public class ActualizarObjetoHandler(IEventStore eventStore) : ICommandHandler<ActualizarObjeto>
{
    public void Handle(ActualizarObjeto command)
    {
        var objetoComplejo = new ObjetoComplejo();
        objetoComplejo.Nombre = "Updated Name";
        objetoComplejo.FechaNacimiento = DateTime.Today;
        objetoComplejo.Lista.Add("New Item");
        objetoComplejo.Monto += 100;

        eventStore.AppendEvent(command.Id, new ObjetoComplejoActualizado(objetoComplejo));
    }
}