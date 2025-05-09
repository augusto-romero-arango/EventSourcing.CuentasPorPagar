using CuentasPorPagar.Dominio;
using CuentasPorPagar.EventStore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Wolverine;
using Wolverine.Marten;

namespace CuentasPorPagar.API;

public static class Extensiones
{
    public static void UsarSerilog(this IHostBuilder builder, string serviceName, string openTelemetryEndpoint)
    {
        builder.UseSerilog((context, configuration) =>
        {
            configuration
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.OpenTelemetry(options =>
                {
                    options.Endpoint = openTelemetryEndpoint;
                    options.ResourceAttributes.Add("service.name", serviceName);
                })
                .Enrich.WithProperty("Application", serviceName);
        });
    }


    public static void AgregarOpenTelemtry(this IServiceCollection services, string serviceName,
        string openTelemetryEndpoint)
    {
        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(serviceName)
            .AddTelemetrySdk();

        services.AddOpenTelemetry()
            .WithTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSource("Marten")
                    .AddSource("Wolverine")
                    .SetResourceBuilder(resourceBuilder)
                    .AddOtlpExporter(options => { options.Endpoint = new Uri(openTelemetryEndpoint); });
            })
            .WithMetrics(metricProviderBuilder =>
            {
                metricProviderBuilder
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddMeter("Marten")
                    .AddMeter("Wolverine")
                    .SetResourceBuilder(resourceBuilder)
                    .AddOtlpExporter(options => { options.Endpoint = new Uri(openTelemetryEndpoint); });
            });
    }

    public static IHealthChecksBuilder AgregarHealthChecks(this IServiceCollection services,
        string martenConnectionString)
    {
        return services
            .AddHealthChecks()
            .AddNpgSql(martenConnectionString);
    }

    public static void UsarWolverine(this IHostBuilder builder, string martenConnectionString, bool isDevelopment)
    {
        builder.UseWolverine(options =>
            {
                options.Discovery.IncludeAssembly(typeof(IEventStore).Assembly);
                options.Services.AgregarConfiguracionMarten(martenConnectionString,
                    isDevelopment).IntegrateWithWolverine();
                options.Policies.AutoApplyTransactions();
                options.Durability.Mode = DurabilityMode.MediatorOnly;
            }
        );
    }

    public static void AgregarWolverineRouter(this IServiceCollection services)
    {
        services.AddScoped<ICommandRouter, WolverineCommandRouter>();
        services.AddScoped<IQueryRouter, WolverineQueryRouter>();
    }
}