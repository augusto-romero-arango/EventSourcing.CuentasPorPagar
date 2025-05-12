using CuentasPorPagar.Dominio.AcreedorCuentaPorPagar;
using CuentasPorPagar.Dominio.Entidades;
using CuentasPorPagar.Dominio.EventosComunes;

namespace CuentasPorPagar.Dominio.ConceptoCuentaPorPagar;

public class AgregarConceptoPorPagarCommandHandler(IEventStore eventStore) : ICommandHandlerAsync<AgregarConceptoPorPagar>
{
    public async Task HandleAsync(AgregarConceptoPorPagar command, CancellationToken cancellationToken)
    {
        if(command.DetallePorPagar.Monto.Valor == 0)
            return;
        
        var cuentaPorPagar = await eventStore.GetAggregateRootAsync<CuentaPorPagar>(command.IdCuentaPorPagar, cancellationToken);
        if (cuentaPorPagar!.Moneda != command.DetallePorPagar.Monto.Moneda)
        {
            eventStore.AppendEvent(command.IdCuentaPorPagar, new CuentaPorPagarIncorrecta( CuentaPorPagarIncorrecta.RazonError.MonedaIncorrecta));
            return;
        }

        eventStore.AppendEvent(command.IdCuentaPorPagar, new ConceptoPorPagarAgregado(command.IdCuentaPorPagar, command.DetallePorPagar));

        var calidadTributariaEmisor = cuentaPorPagar.Acreedor?.CalidadTributaria;
        
        ImpuestoBase[]? impuestosAAplicar = command.DetallePorPagar.ConceptoPorPagar.ImpuestosAAplicar;
        if (impuestosAAplicar != null)
        {
            foreach (var impuesto in impuestosAAplicar)
            {
                ImpuestoAplicado? impuestoAplicado = impuesto.ImpuestoAplicado(command, impuesto, calidadTributariaEmisor);
               
                if (impuestoAplicado != null)
                    eventStore.AppendEvent(command.IdCuentaPorPagar, impuestoAplicado);
            }
        }
    }

    
}