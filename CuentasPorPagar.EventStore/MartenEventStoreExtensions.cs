using Marten;
using Marten.Events;
using Marten.Services;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Core;
using IEventStore = CuentasPorPagar.Dominio.IEventStore;

namespace CuentasPorPagar.EventStore;

public static class MartenEventStoreExtensions
{
    public static MartenServiceCollectionExtensions.MartenConfigurationExpression AgregarConfiguracionMarten(
        this IServiceCollection service, string connectionString,
        bool isDevelopment)
    {
        return service.AddMarten(options =>
        {
            options.Connection(connectionString);
            options.UseSystemTextJsonForSerialization();
            options.Events.StreamIdentity = StreamIdentity.AsGuid;

            if (isDevelopment)
            {
                options.OpenTelemetry.TrackConnections = TrackLevel.Verbose;
                options.AutoCreateSchemaObjects = AutoCreate.All;
            }
            else
            {
                options.DisableNpgsqlLogging = true;
            }
        }).UseLightweightSessions();
    }

    public static void AgregarMartenEventStore(this IServiceCollection service)
    {
        service.AddScoped<IEventStore, MartenEventStore>();
    }
}