using CuentasPorPagar.Dominio;
using Marten;

namespace CuentasPorPagar.EventStore;

public class MartenEventStore(IDocumentSession session, IQuerySession querySession) : IEventStore
{
    public Task<TAggregateRoot?> GetAggregateRootAsync<TAggregateRoot>(Guid aggregateId,
        CancellationToken cancellationToken = default)
        where TAggregateRoot : AggregateRoot, new()
    {
        return querySession.Events.AggregateStreamAsync<TAggregateRoot>(aggregateId, token: cancellationToken);
    }

    public void AppendEvent(Guid aggregateId, object eventData)
    {
        session.Events.Append(aggregateId, eventData);
    }

    public Task SaveChangesAsync()
    {
        return session.SaveChangesAsync();
    }
}