using CuentasPorPagar.API;
using CuentasPorPagar.EventStore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Testcontainers.PostgreSql;
using Wolverine.Marten;

namespace CuentasPorPagar.AcceptanceTests;

public class ApiFactory : WebApplicationFactory<IApiAssemblyMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresSqlContainer = new PostgreSqlBuilder().Build();

    public async Task InitializeAsync()
    {
        await _postgresSqlContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureTestServices(services =>
            services.AgregarConfiguracionMarten(_postgresSqlContainer.GetConnectionString(), true)
                .IntegrateWithWolverine());
    }


    public Task DisposeAsync()
    {
        return _postgresSqlContainer.DisposeAsync().AsTask();
    }
}