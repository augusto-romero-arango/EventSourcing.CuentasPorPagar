namespace CuentasPorPagar.Dominio.IdentificadorCuentaPorPagar;

public class AgregarIdentificacionObligacionHandler(IEventStore eventStore) : ICommandHandler<AgregarIdentificacionObligacion>
{
    public void Handle(AgregarIdentificacionObligacion command)
    {
        eventStore.AppendEvent(command.IdCuentaPorPagar, new IdentificacionObligacionAgregado(command.IdCuentaPorPagar, command.Identificador));
    }
}