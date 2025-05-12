using CuentasPorPagar.Dominio.Entidades;

namespace CuentasPorPagar.Dominio.ConceptoCuentaPorPagar;

public record ConceptoPorPagarAgregado(Guid IdCuentaPorPagar, DetallePorPagar DetallePorPagar);

public record ImpuestoAplicado(Guid IdCuentaPorPagar, Impuesto Impuesto);