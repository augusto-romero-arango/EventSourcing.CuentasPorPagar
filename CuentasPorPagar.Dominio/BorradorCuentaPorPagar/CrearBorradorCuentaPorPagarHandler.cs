namespace CuentasPorPagar.Dominio.BorradorCuentaPorPagar;

public class CrearBorradorCuentaPorPagarHandler(IEventStore eventStore) : ICommandHandler<CrearBorradorCuentaPorPagar>
{
    public void Handle(CrearBorradorCuentaPorPagar command)
    {
        eventStore.AppendEvent(command.IdCuentaPorPagar, new BorradorCuentaPorPagarCreado(command.IdCuentaPorPagar, command.Fecha, command.Moneda));
    }
}