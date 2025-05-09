using CuentasPorPagar.Dominio.ConceptoCuentaPorPagar;

namespace CuentasPorPagar.Dominio.BorradorCuentaPorPagar;

public record CrearBorradorCuentaPorPagar(Guid IdCuentaPorPagar, DateOnly Fecha, Moneda Moneda);