using CuentasPorPagar.API;
using CuentasPorPagar.Dominio;
using CuentasPorPagar.EventStore;

const string serviceName = "CuentasPorPagar";

var builder = WebApplication.CreateBuilder(args);

var martenConnectionString = builder.Configuration.GetConnectionString("MartenEventStore") ??
                             throw new ArgumentNullException(
                                 $"La cadena de conexión 'MartenEventStore' no está configurada.");
var openTelemetryEndpoint = builder.Configuration.GetValue<string>("OpenTelemetryEndpoint") ??
                            throw new ArgumentNullException(
                                $"La url de OpenTelemtry no está configurada.");
;

builder.Host.UsarSerilog(serviceName, openTelemetryEndpoint);
builder.Host.UsarWolverine(martenConnectionString, builder.Environment.IsDevelopment());

builder.Services.AddOpenApi();

builder.Services.AgregarOpenTelemtry(serviceName, openTelemetryEndpoint);
builder.Services.AgregarHealthChecks(martenConnectionString);
builder.Services.AgregarMartenEventStore();
builder.Services.AgregarWolverineRouter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.MapOpenApi();

// app.UseHttpsRedirection();
app.UseHealthChecks("/health");

app.MapPost("/product", async (CreateProduct command, ICommandRouter router) =>
    {
        var id = await router.InvokeAsync<CreateProduct, Guid>(command);
        return Results.Created($"/product/{id}", id);
    })
    .WithSummary("Create Product")
    .WithDescription("Create a new product")
    .WithTags("Products")
    .WithName("CreateProduct")
    .Produces<Guid>(StatusCodes.Status201Created);

app.MapGet("/product/{id:guid}", async (Guid id, IEventStore eventStore) =>
    {
        ProductAggregate? producto = await eventStore.GetAggregateRootAsync<ProductAggregate>(id);
        return Results.Ok(producto);
    })
    .WithSummary("Get Product")
    .WithDescription("Get a product by id")
    .WithTags("Products")
    .WithName("GetProduct")
    .Produces<ProductAggregate>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

app.Run();

public record CreateProduct(string Name, string Description, decimal Price);

public record ProductCreated(Guid Id, string Name, string Description, decimal Price);

public class CreateProductHandler(IEventStore eventStore) : ICommandHandler<CreateProduct, Guid>
{
    public Guid Handle(CreateProduct command)
    {
        var productId = Guid.NewGuid();
        var @event = new ProductCreated(
            productId,
            command.Name,
            command.Description,
            command.Price
        );

        eventStore.AppendEvent(productId, @event);

        return productId;
    }
}

public class ProductAggregate : AggregateRoot
{
    public void Apply(ProductCreated @event)
    {
        Id = @event.Id;
        Name = @event.Name;
        Description = @event.Description;
        Price = @event.Price;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public decimal Price { get; private set; }
}