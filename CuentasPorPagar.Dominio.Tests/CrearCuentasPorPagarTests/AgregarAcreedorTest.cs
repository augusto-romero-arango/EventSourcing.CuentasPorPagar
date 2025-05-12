using CuentasPorPagar.Dominio.AcreedorCuentaPorPagar;
using CuentasPorPagar.Dominio.BorradorCuentaPorPagar;
using CuentasPorPagar.Dominio.ConceptoCuentaPorPagar;
using CuentasPorPagar.Dominio.Entidades;

namespace CuentasPorPagar.Dominio.Tests.CrearCuentasPorPagarTests;

public class AgregarAcreedorTest : CommandHandlerTest<AgregarAcreedor>
{
    protected override ICommandHandler<AgregarAcreedor> Handler=> 
        new AgregarAcreedorHandler(eventStore);

    [Fact]
    public void Si_AgregarAcreedor_CuentaPorPagarRegistraAcreedor()
    {
        Given(new CrearBorradorCuentaPorPagar(_aggregateId, new DateOnly(2025, 5, 9),Moneda.COP));
        var acreedor = new Acreedor(TipoDocumento.Nit, "1111111", "Acreedor 1");
        When(new AgregarAcreedor(_aggregateId, acreedor));
        Then(new AcreedorAgregado(_aggregateId, acreedor));
        And<CuentaPorPagar>(cxp => cxp.Acreedor!, acreedor);

    }
    
    [Fact]
    public void Si_AgregarAcreedor_CuentaPorPagarRegistraAcreedor_ResponsableIva()
    {
        Given(new CrearBorradorCuentaPorPagar(_aggregateId, new DateOnly(2025, 5, 9),Moneda.COP));
        var acreedor = new Acreedor(TipoDocumento.Nit, "1111111", "Acreedor 1", new ResponsableIva());
        When(new AgregarAcreedor(_aggregateId, acreedor));
        Then(new AcreedorAgregado(_aggregateId, acreedor));
        And<CuentaPorPagar>(cxp => cxp.Acreedor!, acreedor);

    }
}

