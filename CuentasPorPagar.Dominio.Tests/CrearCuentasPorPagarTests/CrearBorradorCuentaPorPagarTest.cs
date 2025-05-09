using CuentasPorPagar.Dominio.BorradorCuentaPorPagar;
using CuentasPorPagar.Dominio.ConceptoCuentaPorPagar;
using CuentasPorPagar.Dominio.Entidades;

namespace CuentasPorPagar.Dominio.Tests.CrearCuentasPorPagarTests;

public class CrearBorradorCuentaPorPagarTest : CommandHandlerTest<CrearBorradorCuentaPorPagar>
{
    protected override ICommandHandler<CrearBorradorCuentaPorPagar> Handler =>
        new CrearBorradorCuentaPorPagarHandler(eventStore);

    [Fact]
    public void Si_SeCreaaUnBorrador_Debe_CrearUnaCuentaPorPagarEnEstadoBorrador()
    {
        When(new CrearBorradorCuentaPorPagar(_aggregateId, new DateOnly(2025, 5, 9), Moneda.COP));
        Then(new BorradorCuentaPorPagarCreado(_aggregateId, new DateOnly(2025, 5, 9), Moneda.COP));
        And<CuentaPorPagar>(cuentaPorPagar =>
            cuentaPorPagar.Estado, EstadoCuentaPorPagar.Borrador);
        And<CuentaPorPagar>(cuentaPorPagar =>
            cuentaPorPagar.Moneda, Moneda.COP);
        And<CuentaPorPagar>(cuentaPorPagar => cuentaPorPagar.Fecha, new DateOnly(2025, 5, 9));
        And<CuentaPorPagar>(cuentaPorPagar => cuentaPorPagar.FechaVencimiento, new DateOnly(2025, 5, 9));
    }
}