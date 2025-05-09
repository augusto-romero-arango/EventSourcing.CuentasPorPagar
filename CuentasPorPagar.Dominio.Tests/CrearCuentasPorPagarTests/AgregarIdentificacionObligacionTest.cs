using CuentasPorPagar.Dominio.BorradorCuentaPorPagar;
using CuentasPorPagar.Dominio.ConceptoCuentaPorPagar;
using CuentasPorPagar.Dominio.Entidades;
using CuentasPorPagar.Dominio.IdentificadorCuentaPorPagar;
using Marten.Events;

namespace CuentasPorPagar.Dominio.Tests.CrearCuentasPorPagarTests;

public class AgregarIdentificacionObligacionTest : CommandHandlerTest<AgregarIdentificacionObligacion>
{
    protected override ICommandHandler<AgregarIdentificacionObligacion> Handler => 
        new AgregarIdentificacionObligacionHandler(eventStore);

    [Fact]
    public void Si_AgregarIdentificacionObligacion_CuentaPorPagarMuestraIdentificacionObligacion()
    {
        Given(new CrearBorradorCuentaPorPagar(_aggregateId, new DateOnly(2025, 5, 9), Moneda.COP));
        When(new AgregarIdentificacionObligacion(_aggregateId, "FV-1"));
        Then(new IdentificacionObligacionAgregado(_aggregateId, "FV-1"));
        And<CuentaPorPagar>(cxp => cxp.IdentificadorObligacion!, "FV-1");
    }
}