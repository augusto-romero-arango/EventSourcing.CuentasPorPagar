namespace CuentasPorPagar.Dominio.Tests;

public class TestStore : IEventStore
{
    private readonly Dictionary<Guid, List<object>> _newEvents = new();
    private readonly Dictionary<Guid, List<object>> _previousEvents = new();

    public void AppendEvent(Guid aggregateId, object eventData)
    {
        var eventos = _newEvents.GetValueOrDefault(aggregateId, []);
        eventos.Add(eventData);
        _newEvents[aggregateId] = eventos;
    }

    public Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<TAggregateRoot?> GetAggregateRootAsync<TAggregateRoot>(Guid aggregateId,
        CancellationToken cancellationToken = default)
        where TAggregateRoot : AggregateRoot, new()
    {
        if (_previousEvents.ContainsKey(aggregateId) == false)
            return Task.FromResult<TAggregateRoot?>(null);

        TAggregateRoot aggregateRoot = new();

        foreach (var @event in _previousEvents[aggregateId]) aggregateRoot.Apply((dynamic) @event);

        return Task.FromResult(aggregateRoot)!;
    }

    public void AppendPreviosEvents(Guid aggregateId, object[] events)
    {
        _previousEvents.Add(aggregateId, events.ToList());
    }

    public IEnumerable<object> GetNewEvents(Guid aggregateId)
    {
        return _newEvents.GetValueOrDefault(aggregateId, []);
    }

    public TAggregateRoot? GetAggregateRoot<TAggregateRoot>(Guid aggregateId)
        where TAggregateRoot : AggregateRoot, new()
    {
        if (_previousEvents.ContainsKey(aggregateId) is false && _newEvents.ContainsKey(aggregateId) is false)
            return null;

        TAggregateRoot aggregateRoot = new();

        foreach (var @event in (List<object>)
                 [
                     .._previousEvents.GetValueOrDefault(aggregateId, []),
                     .._newEvents.GetValueOrDefault(aggregateId, [])
                 ])
            aggregateRoot.Apply((dynamic) @event);

        return aggregateRoot;
    }
}