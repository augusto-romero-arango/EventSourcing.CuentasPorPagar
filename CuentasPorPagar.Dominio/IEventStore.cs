namespace CuentasPorPagar.Dominio;

public interface IEventStore
{
    void AppendEvent(Guid aggregateId, object eventData);
    Task SaveChangesAsync();

    Task<TAggregateRoot?> GetAggregateRootAsync<TAggregateRoot>(Guid aggregateId,
        CancellationToken cancellationToken = default) where TAggregateRoot : AggregateRoot, new();
}