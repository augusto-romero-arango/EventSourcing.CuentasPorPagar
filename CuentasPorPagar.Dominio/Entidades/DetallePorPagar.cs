using CuentasPorPagar.Dominio.ConceptoCuentaPorPagar;

namespace CuentasPorPagar.Dominio.Entidades;

public record ConceptoPorPagar(string Codigo, string Descripcion, IImpuesto[]? ImpuestosAAplicar = null);

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

public interface IImpuesto
{
    string Descripcion { get; }
    decimal Tasa { get; }
    
}

public record Iva19 : IImpuesto
{
    public string Descripcion => "IVA 19%";
    public decimal Tasa => 0.19m;
}
public record Inc8 : IImpuesto
{
    public string Descripcion => "INC 8%";
    public decimal Tasa => 0.08m;
}