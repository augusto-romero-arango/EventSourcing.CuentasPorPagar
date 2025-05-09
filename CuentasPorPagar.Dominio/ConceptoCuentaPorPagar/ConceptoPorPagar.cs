using System.Runtime.CompilerServices;

namespace CuentasPorPagar.Dominio.ConceptoCuentaPorPagar;

public record ConceptoPorPagar(
    Guid IdConcepto,
    string Codigo,
    string Descripcion,
    CentroCosto? CentroCosto,
    Dinero Monto);

public record CentroCosto(string Id, string Descripcion);

public record Dinero
{
    private Dinero(decimal valor, Moneda moneda)
    {
        Valor = valor;
        Moneda = moneda;
    }

    public static Dinero COP(decimal valor) => new(valor, ConceptoCuentaPorPagar.Moneda.COP);
    public static Dinero USD(decimal valor) => new( valor, ConceptoCuentaPorPagar.Moneda.USD);
    public static Dinero Cero(Moneda moneda) => new(0, moneda);
    public decimal Valor { get; init; }
    public Moneda Moneda { get; init; }

    public static Dinero operator +(Dinero sumando1, Dinero sumando2)
    {
        if (sumando1.Moneda != sumando2.Moneda)
            throw new InvalidOperationException("No se pueden operar montos de diferentes monedas");
            
        return new Dinero(sumando1.Valor + sumando2.Valor, sumando1.Moneda);
    }
    
    public static Dinero operator -(Dinero minuendo, Dinero sustraendo)
    {
        if (minuendo.Moneda != sustraendo.Moneda)
            throw new InvalidOperationException("No se pueden operar montos de diferentes monedas");
        return new Dinero(minuendo.Valor - sustraendo.Valor, minuendo.Moneda);
    }
}

public enum Moneda
{
    COP = 0,
    USD = 1,
}
public static class DineroExtensions
{
    public static Dinero Sumar(this IEnumerable<Dinero> sumandos, Moneda moneda)
    {
        
        return sumandos.Aggregate(Dinero.Cero(moneda),  (suma, sumando) => suma + sumando);
    }
}