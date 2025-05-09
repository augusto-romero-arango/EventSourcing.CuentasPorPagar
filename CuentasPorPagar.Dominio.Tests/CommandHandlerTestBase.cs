using FluentAssertions;
using FluentAssertions.Equivalency;

namespace CuentasPorPagar.Dominio.Tests;

public class CommandHandlerTestBase
{
    /// <summary>
    ///     If no explicit aggregateId is provided, this one will be used behind the scenes.
    /// </summary>
    protected readonly Guid _aggregateId = Guid.NewGuid();

    /// <summary>
    ///     A fake, in-memory event store.
    /// </summary>
    protected TestStore eventStore = new();

    /// <summary>
    ///     Sets a list of previous events for a specified aggregate ID.
    /// </summary>
    protected void Given(Guid aggregateId, params object[] events)
    {
        eventStore.AppendPreviosEvents(aggregateId, events);
    }

    /// <summary>
    ///     Sets a list of previous events for the default aggregate ID.
    /// </summary>
    protected void Given(params object[] events)
    {
        Given(_aggregateId, events);
    }

    /// <summary>
    ///     Asserts that the expected events have been appended to the event store
    ///     for the default aggregate ID.
    /// </summary>
    protected void Then(params object[] expectedEvents)
    {
        Then(_aggregateId, expectedEvents);
    }

    /// <summary>
    ///     Asserts that the expected events have been appended to the event store
    ///     for a specific aggregate ID.
    /// </summary>
    protected void Then(Guid aggregateId, params object[] expectedEvents)
    {
        var newEvents = eventStore.GetNewEvents(aggregateId).ToList();

        newEvents.Count.Should().Be(expectedEvents.Length);

        for (var i = 0; i < newEvents.Count; i++)
        {
            newEvents[i].Should().BeOfType(expectedEvents[i].GetType());
            try
            {
                newEvents[i].Should().BeEquivalentTo(expectedEvents[i]);
            }
            catch (InvalidOperationException e)
            {
                // Empty event with matching type is OK. This means that the event class
                //  has no properties. If the types match in this situation, the correct
                // event has been appended. So we should ignore this exception.
                if (!e.Message.StartsWith("No members were found for comparison."))
                    throw;
            }
        }
    }

    /// <summary>
    ///     Asserts that a property of an aggregate root matches the expected value using equivalency options.
    /// </summary>
    /// <param name="contextFunc"></param>
    /// <param name="expectedValue"></param>
    /// <param name="options"></param>
    /// <typeparam name="TAggregateRoot"></typeparam>
    public void And<TAggregateRoot>(Func<TAggregateRoot, object> contextFunc, object expectedValue,
        Func<EquivalencyOptions<object>, EquivalencyOptions<object>>? options = null)
        where TAggregateRoot : AggregateRoot, new()
    {
        And<TAggregateRoot, object>(contextFunc, expectedValue, options);
    }

    /// <summary>
    ///     Asserts that a property of an aggregate root matches the expected value using equivalency options.
    /// </summary>
    /// <param name="contextFunc"></param>
    /// <param name="expectedValue"></param>
    /// <param name="options"></param>
    /// <typeparam name="TAggregateRoot"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public void And<TAggregateRoot, TResult>(Func<TAggregateRoot, TResult> contextFunc, TResult expectedValue,
        Func<EquivalencyOptions<TResult>, EquivalencyOptions<TResult>>? options = null)
        where TAggregateRoot : AggregateRoot, new()
    {
        var entidad = eventStore.GetAggregateRoot<TAggregateRoot>(_aggregateId);
        ArgumentNullException.ThrowIfNull(entidad);
        var opciones = options ?? (opt => opt);
        contextFunc(entidad).Should().BeEquivalentTo(expectedValue, opciones);
    }
}