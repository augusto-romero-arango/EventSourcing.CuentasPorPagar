using CuentasPorPagar.Dominio.ConceptoCuentaPorPagar;

namespace CuentasPorPagar.Dominio.BorradorCuentaPorPagar;

public record BorradorCuentaPorPagarCreado(Guid IdCuentaPorPagar, DateOnly Fecha, Moneda Moneda);

