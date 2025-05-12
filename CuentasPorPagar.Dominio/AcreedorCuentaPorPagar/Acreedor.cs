namespace CuentasPorPagar.Dominio.AcreedorCuentaPorPagar;

public record Acreedor(TipoDocumento TipoDocumento, string NumeroDocumento, string RazonSocial, ICalidadTributaria? CalidadTributaria = null);

public record ResponsableIva : ICalidadTributaria;

public interface ICalidadTributaria;