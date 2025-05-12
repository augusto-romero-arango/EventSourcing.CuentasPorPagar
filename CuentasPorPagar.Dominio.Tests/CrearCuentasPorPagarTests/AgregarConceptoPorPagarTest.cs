using CuentasPorPagar.Dominio.AcreedorCuentaPorPagar;
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
        var concepto = new DetallePorPagar(Guid.NewGuid(), new ConceptoPorPagar("190.1", "Ventanas"), new CentroCosto("0101", "CC 0101"), Dinero.COP(1_000_000m));
        
        await WhenAsync(new AgregarConceptoPorPagar(_aggregateId, concepto));
        Then(new ConceptoPorPagarAgregado(_aggregateId, concepto));
        And<CuentaPorPagar>(cxp => cxp.ConceptosPorPagar,  new List<DetallePorPagar> {concepto});
        And<CuentaPorPagar>(cxp => cxp.Saldo, Dinero.COP(1_000_000));
    }

    [Fact]
    public async Task Si_AgregaConceptoPorPagarConMontoCero_NoTieneEnCuentaElConcepto()
    {
        Given(new CrearBorradorCuentaPorPagar(_aggregateId, new DateOnly(2025, 5, 9), Moneda.COP));
        var concepto = new DetallePorPagar(Guid.NewGuid(), new ConceptoPorPagar("190.1", "Ventanas"), new CentroCosto("0101", "CC 0101"), Dinero.COP(0));
        
        await WhenAsync(new AgregarConceptoPorPagar(_aggregateId, concepto));
        And<CuentaPorPagar>(cxp => cxp.ConceptosPorPagar,  new List<DetallePorPagar>());
        And<CuentaPorPagar>(cxp => cxp.Saldo, Dinero.COP(0));
    }

    [Fact]
    public async Task Si_AgregaConceptoPorPagarConMonedaDiferenteAlDocumento_DebeEmitirEventoDeFallo()
    {
        Given(new CrearBorradorCuentaPorPagar(_aggregateId, new DateOnly(2025, 5, 9), Moneda.COP));
        var concepto = new DetallePorPagar(Guid.NewGuid(), new ConceptoPorPagar("190.1", "Ventanas"), new CentroCosto("0101", "CC 0101"), Dinero.USD(1_000));
        
        await WhenAsync(new AgregarConceptoPorPagar(_aggregateId, concepto));
        Then(new CuentaPorPagarIncorrecta(CuentaPorPagarIncorrecta.RazonError.MonedaIncorrecta));
    }

    [Fact]
    public async Task Si_ConceptoPorPagarEsGravadoConIVA19_AcreedorEsResponsableDeIVA_DebeCalcularIVA()
    {
        Given(new CrearBorradorCuentaPorPagar(_aggregateId, new DateOnly(2025, 5, 9), Moneda.COP),
            new AcreedorAgregado(_aggregateId, new Acreedor(TipoDocumento.Nit, "1111111", "Acreedor 1", new ResponsableIva())));
        
        var conceptoGravado = new ConceptoPorPagar("190.1", "Ventanas", [new Iva19()]);
        var centroCosto = new CentroCosto("0101", "CC 0101");
        var concepto = new DetallePorPagar(Guid.NewGuid(), conceptoGravado, centroCosto, Dinero.COP(1_000_000m));
        
        await WhenAsync(new AgregarConceptoPorPagar(_aggregateId, concepto));
        
        Then(new ConceptoPorPagarAgregado(_aggregateId, concepto), 
            new ImpuestoAplicado(_aggregateId, new Impuesto(concepto.IdConcepto, "IVA 19%", 0.19m, Dinero.COP(1_000_000), Dinero.COP(190_000))));
        And<CuentaPorPagar>(cxp => cxp.ConceptosPorPagar,  new List<DetallePorPagar>
        {
            new(concepto.IdConcepto, conceptoGravado, centroCosto, concepto.Monto, 
                [new ImpuestoAPagar(Dinero.COP(1_000_000), 0.19m, Dinero.COP(190_000) )])
        });
    }
    
    [Fact]
    public async Task Si_ConceptoPorPagarEsGravadoConIVA19_AcreedorNoEsResponsableDeIVA_NoDebeCalcularIVA()
    {
        Given(new CrearBorradorCuentaPorPagar(_aggregateId, new DateOnly(2025, 5, 9), Moneda.COP),
            new AcreedorAgregado(_aggregateId, new Acreedor(TipoDocumento.Nit, "1111111", "Acreedor 1")));
        
        var conceptoGravado = new ConceptoPorPagar("190.1", "Ventanas", [new Iva19()]);
        var centroCosto = new CentroCosto("0101", "CC 0101");
        var concepto = new DetallePorPagar(Guid.NewGuid(), conceptoGravado, centroCosto, Dinero.COP(1_000_000m));
        
        await WhenAsync(new AgregarConceptoPorPagar(_aggregateId, concepto));
        
        Then(new ConceptoPorPagarAgregado(_aggregateId, concepto));
        And<CuentaPorPagar>(cxp => cxp.ConceptosPorPagar,  new List<DetallePorPagar>
        {
            new(concepto.IdConcepto, conceptoGravado, centroCosto, concepto.Monto)
        });
    }
    
    [Fact]
    public async Task Si_ConceptoPorPagarEsGravadoConImpuestoConsumo8_AcreedorNoEsResponsableDeIVA_DebeCalcularImpuestoConsumo()
    {
        Given(new CrearBorradorCuentaPorPagar(_aggregateId, new DateOnly(2025, 5, 9), Moneda.COP),
            new AcreedorAgregado(_aggregateId, new Acreedor(TipoDocumento.Nit, "1111111", "Acreedor 1")));
        
        var conceptoGravado = new ConceptoPorPagar("190.1", "Ventanas", [new Inc8()]);
        var centroCosto = new CentroCosto("0101", "CC 0101");
        var concepto = new DetallePorPagar(Guid.NewGuid(), conceptoGravado, centroCosto, Dinero.COP(1_000_000m));
        
        await WhenAsync(new AgregarConceptoPorPagar(_aggregateId, concepto));
        
        Then(new ConceptoPorPagarAgregado(_aggregateId, concepto), 
            new ImpuestoAplicado(_aggregateId, new Impuesto(concepto.IdConcepto, "INC 8%", 0.08m, Dinero.COP(1_000_000), Dinero.COP(80_000))));
        And<CuentaPorPagar>(cxp => cxp.ConceptosPorPagar,  new List<DetallePorPagar>
        {
            new(concepto.IdConcepto, conceptoGravado, centroCosto, concepto.Monto, 
                [new ImpuestoAPagar(Dinero.COP(1_000_000), 0.08m, Dinero.COP(80_000) )])
        });
    }
    
    [Fact]
    public async Task Si_ConceptoPorPagarEsGravadoConImpuestoConsumo8_AcreedorEsResponsableDeIVA_DebeCalcularImpuestoConsumo()
    {
        Given(new CrearBorradorCuentaPorPagar(_aggregateId, new DateOnly(2025, 5, 9), Moneda.COP),
            new AcreedorAgregado(_aggregateId, new Acreedor(TipoDocumento.Nit, "1111111", "Acreedor 1", new ResponsableIva())));;
        
        var conceptoGravado = new ConceptoPorPagar("190.1", "Ventanas", [new Inc8()]);
        var centroCosto = new CentroCosto("0101", "CC 0101");
        var concepto = new DetallePorPagar(Guid.NewGuid(), conceptoGravado, centroCosto, Dinero.COP(1_000_000m));
        
        await WhenAsync(new AgregarConceptoPorPagar(_aggregateId, concepto));
        
        Then(new ConceptoPorPagarAgregado(_aggregateId, concepto), 
            new ImpuestoAplicado(_aggregateId, new Impuesto(concepto.IdConcepto, "INC 8%", 0.08m, Dinero.COP(1_000_000), Dinero.COP(80_000))));
        And<CuentaPorPagar>(cxp => cxp.ConceptosPorPagar,  new List<DetallePorPagar>
        {
            new(concepto.IdConcepto, conceptoGravado, centroCosto, concepto.Monto, 
                [new ImpuestoAPagar(Dinero.COP(1_000_000), 0.08m, Dinero.COP(80_000) )])
        });
    }
    
}

