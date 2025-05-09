using CuentasPorPagar.Dominio.BorradorCuentaPorPagar;
using CuentasPorPagar.Dominio.ConceptoCuentaPorPagar;
using CuentasPorPagar.Dominio.Entidades;
using CuentasPorPagar.Dominio.EventosComunes;

namespace CuentasPorPagar.Dominio.Tests.CrearCuentasPorPagarTests;

public class AgregarConceptoPorPagarTest : CommandHandlerAsyncTest<AgregarConceptoPorPagar>
{
    protected override ICommandHandlerAsync<AgregarConceptoPorPagar> Handler => 
        new AgregarConceptoPorPagarCommandHandler(eventStore);

    [Fact]
    public async Task Si_AgregaConceptoPorPagar_CreaConceptoYCalculaSaldoAPagar()
    {
        Given(new CrearBorradorCuentaPorPagar(_aggregateId, new DateOnly(2025, 5, 9), Moneda.COP));
        var concepto = new ConceptoPorPagar(Guid.NewGuid(), "190.1", "Ventanas", new CentroCosto("0101", "CC 0101"), Dinero.COP(1_000_000m));
        
        await WhenAsync(new AgregarConceptoPorPagar(_aggregateId, concepto));
        Then(new ConceptoPorPagarAgregado(_aggregateId, concepto));
        And<CuentaPorPagar>(cxp => cxp.ConceptosPorPagar,  new List<ConceptoPorPagar> {concepto});
        And<CuentaPorPagar>(cxp => cxp.Saldo, Dinero.COP(1_000_000));
    }

    [Fact]
    public async Task Si_AgregaConceptoPorPagarConMontoCero_NoTieneEnCuentaElConcepto()
    {
        Given(new CrearBorradorCuentaPorPagar(_aggregateId, new DateOnly(2025, 5, 9), Moneda.COP));
        var concepto = new ConceptoPorPagar(Guid.NewGuid(), "190.1", "Ventanas", new CentroCosto("0101", "CC 0101"), Dinero.COP(0));
        
        await WhenAsync(new AgregarConceptoPorPagar(_aggregateId, concepto));
        And<CuentaPorPagar>(cxp => cxp.ConceptosPorPagar,  new List<ConceptoPorPagar>());
        And<CuentaPorPagar>(cxp => cxp.Saldo, Dinero.COP(0));
    }

    [Fact]
    public async Task Si_AgregaConceptoPorPagarConMonedaDiferenteAlDocumento_DebeEmitirEventoDeFallo()
    {
        Given(new CrearBorradorCuentaPorPagar(_aggregateId, new DateOnly(2025, 5, 9), Moneda.COP));
        var concepto = new ConceptoPorPagar(Guid.NewGuid(), "190.1", "Ventanas", new CentroCosto("0101", "CC 0101"), Dinero.USD(1_000));
        
        await WhenAsync(new AgregarConceptoPorPagar(_aggregateId, concepto));
        Then(new CuentaPorPagarIncorrecta(CuentaPorPagarIncorrecta.RazonError.MonedaIncorrecta));
    }
}