using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace CuentasPorPagar.AcceptanceTests;

public class EjemploUnitTest : IClassFixture<ApiFactory>
{
    private readonly HttpClient _cliente;

    public EjemploUnitTest(ApiFactory apiFactory)
    {
        _cliente = apiFactory.CreateClient();
    }

    [Fact]
    public async Task Ejemplo1()
    {
        var response = await _cliente.PostAsJsonAsync("/product", new CreateProduct("nombre", "Descripcion", 100));

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var id = await response.Content.ReadFromJsonAsync<Guid>();

        var response2 = await _cliente.GetAsync($"/product/{id}");
        response2.StatusCode.Should().Be(HttpStatusCode.OK);

        var respuestaProducto = await response2.Content.ReadFromJsonAsync<ProductCreated>();
        respuestaProducto.Should().NotBeNull();
        respuestaProducto.Id.Should().Be(id);
        respuestaProducto.Name.Should().Be("nombre");
    }
}