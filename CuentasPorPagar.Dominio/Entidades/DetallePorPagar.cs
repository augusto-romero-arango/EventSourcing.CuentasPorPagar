using CuentasPorPagar.Dominio.AcreedorCuentaPorPagar;
using CuentasPorPagar.Dominio.ConceptoCuentaPorPagar;

namespace CuentasPorPagar.Dominio.Entidades;

public record ConceptoPorPagar(string Codigo, string Descripcion, ImpuestoBase[]? ImpuestosAAplicar = null);

public record DetallePorPagar(
    Guid IdConcepto,
    ConceptoPorPagar ConceptoPorPagar,
    CentroCosto? CentroCosto,
    Dinero Monto,
    List<ImpuestoAPagar>? ImpuestosAPagar = null)
{
    public List<ImpuestoAPagar>? ImpuestosAPagar { get; init; } = ImpuestosAPagar ?? [];

    public void AgregarImpuesto(Impuesto eventImpuesto)
    {
        ImpuestosAPagar!.Add(new ImpuestoAPagar(eventImpuesto.Base, eventImpuesto.Tasa, eventImpuesto.ValorAPagar));
    }
}

public record ImpuestoAPagar(Dinero Base, decimal Tasa, Dinero ValorImpuesto);

public record Impuesto(Guid IdConceptoPorPagar, string Descripcion, decimal Tasa, Dinero Base, Dinero ValorAPagar);

public abstract class ImpuestoBase
{
    protected abstract string Descripcion { get; }
    protected abstract decimal Tasa { get; }
    protected abstract bool DebeAplicarImpuesto(ICalidadTributaria? calidadTributariaEmisor, Dinero baseImpuesto);
    public ImpuestoAplicado? ImpuestoAplicado(AgregarConceptoPorPagar command, ImpuestoBase impuestoBase, ICalidadTributaria? calidadTributariaEmisor)
    {
        if(DebeAplicarImpuesto(calidadTributariaEmisor, command.DetallePorPagar.Monto))
            return new ImpuestoAplicado(command.IdCuentaPorPagar,
                new Impuesto(command.DetallePorPagar.IdConcepto,
                    impuestoBase.Descripcion,
                    impuestoBase.Tasa,
                    command.DetallePorPagar.Monto,
                    command.DetallePorPagar.Monto * impuestoBase.Tasa));
        
        return null;
    }
}

public class Iva19 : ImpuestoBase
{
    protected override string Descripcion => "IVA 19%";
    protected override decimal Tasa => 0.19m;

    protected override bool DebeAplicarImpuesto(ICalidadTributaria? calidadTributariaEmisor, Dinero baseImpuesto)
    {
        return calidadTributariaEmisor is ResponsableIva;
    }

   
}

public class Inc8 : ImpuestoBase
{
    protected override string Descripcion => "INC 8%";
    protected override decimal Tasa => 0.08m;

    protected override bool DebeAplicarImpuesto(ICalidadTributaria? calidadTributariaEmisor, Dinero baseImpuesto)
    {
        return true;
    }

    
}

public class IvaTecnologia19 : ImpuestoBase
{
    protected override string Descripcion => "IVA Tecnología 19%";
    protected override decimal Tasa => 0.19m;
    private Dinero Tope => Dinero.COP(2_000_000);
    protected override bool DebeAplicarImpuesto(ICalidadTributaria? calidadTributariaEmisor, Dinero baseImpuesto)
    {
        if (calidadTributariaEmisor is not ResponsableIva)
            return false;

        return baseImpuesto > Tope;
    }
}