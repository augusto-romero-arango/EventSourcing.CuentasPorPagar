using CuentasPorPagar.Dominio.AcreedorCuentaPorPagar;
using CuentasPorPagar.Dominio.BorradorCuentaPorPagar;
using CuentasPorPagar.Dominio.ConceptoCuentaPorPagar;
using CuentasPorPagar.Dominio.IdentificadorCuentaPorPagar;

namespace CuentasPorPagar.Dominio.Entidades;

public class CuentaPorPagar : AggregateRoot
{
    public void Apply(BorradorCuentaPorPagarCreado @event)
    {
        Id = @event.IdCuentaPorPagar;
        Estado = EstadoCuentaPorPagar.Borrador;
        Moneda = @event.Moneda;
        Fecha = @event.Fecha;
        FechaVencimiento = Fecha;
    }
    public void Apply(AcreedorAgregado @event)
    {
        Acreedor = @event.Acreedor;
    }

    public void Apply(IdentificacionObligacionAgregado @event)
    {
        IdentificadorObligacion = @event.IdentificadorObligacion;
    }

    public void Apply(ConceptoPorPagarAgregado @event)
    {
        ConceptosPorPagar.Add(@event.DetallePorPagar);
    }

    public void Apply(ImpuestoAplicado @event)
    {
        var concepto = ConceptosPorPagar
            .First(c => c.IdConcepto== @event.Impuesto.IdConceptoPorPagar);
        concepto.AgregarImpuesto(@event.Impuesto);
    }
    
    public DateOnly Fecha { get; private set; }
    public DateOnly FechaVencimiento { get; private set; }
    public Moneda Moneda { get; private set; }
    public EstadoCuentaPorPagar Estado { get; private set; }
    public Guid Id { get; private set; }
    public Acreedor? Acreedor { get; private  set; }
    public string? IdentificadorObligacion { get; private set; }
    public List<DetallePorPagar> ConceptosPorPagar { get; private set; } = new();
    public Dinero Saldo => ConceptosPorPagar
        .Select(c => c.Monto)
        .Sumar(Moneda);

    
}

public enum EstadoCuentaPorPagar
{
    Borrador = 0
}