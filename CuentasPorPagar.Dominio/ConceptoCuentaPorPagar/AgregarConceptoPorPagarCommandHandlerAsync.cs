using CuentasPorPagar.Dominio.Entidades;
using CuentasPorPagar.Dominio.EventosComunes;

namespace CuentasPorPagar.Dominio.ConceptoCuentaPorPagar;

public class AgregarConceptoPorPagarCommandHandler(IEventStore eventStore) : ICommandHandlerAsync<AgregarConceptoPorPagar>
{
    public async Task HandleAsync(AgregarConceptoPorPagar command, CancellationToken cancellationToken)
    {
        if(command.ConceptoPorPagar.Monto.Valor == 0)
            return;
        
        var cuentaPorPagar = await eventStore.GetAggregateRootAsync<CuentaPorPagar>(command.IdCuentaPorPagar, cancellationToken);
        if (cuentaPorPagar!.Moneda != command.ConceptoPorPagar.Monto.Moneda)
        {
            eventStore.AppendEvent(command.IdCuentaPorPagar, new CuentaPorPagarIncorrecta( CuentaPorPagarIncorrecta.RazonError.MonedaIncorrecta));
            return;
        }
            
        eventStore.AppendEvent(command.IdCuentaPorPagar, new ConceptoPorPagarAgregado(command.IdCuentaPorPagar, command.ConceptoPorPagar));
    }
}