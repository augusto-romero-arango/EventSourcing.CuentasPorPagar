namespace CuentasPorPagar.Dominio.AcreedorCuentaPorPagar;

public class AgregarAcreedorHandler(IEventStore eventStore) : ICommandHandler<AgregarAcreedor>
{
    public void Handle(AgregarAcreedor command)
    {
        eventStore.AppendEvent(command.IdCuentaPorPagar, new AcreedorAgregado(command.IdCuentaPorPagar, command.Acreedor));
    }
}