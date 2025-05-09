namespace CuentasPorPagar.Dominio.EventosComunes;

public record CuentaPorPagarIncorrecta(CuentaPorPagarIncorrecta.RazonError razon)
{
    public enum RazonError
    {
        MonedaIncorrecta = 0,
    }
}