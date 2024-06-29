using System.Data;
using Core.Aggregates;
using Core.Repository;

namespace Core.Event;

public class MongoEventStore(IEventStoreRepository eventStoreRepository) : IEventStore
{
    public void SaveEvents(Guid aggregateId, string aggregateType, List<BaseEvent> uncommittedChanges,
        int expectedVersion, bool constructor = false)
    {
        var eventStream = eventStoreRepository.FindByAggregateId(aggregateId);

        if (constructor)
        {
            if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion)
                throw new ConcurrencyException();
        }

        var version = expectedVersion;

        foreach (var @event in uncommittedChanges)
        {
            version++;
            @event.Version = version;
            var eventType = @event.GetType().Name;
            var eventModel = new EventModel
            {
                TimeStamp = DateTime.Now,
                AggregateIdentifier = aggregateId,
                AggregateType = aggregateType,
                Version = version,
                EventType = eventType,
                Event = @event
            };
            eventStoreRepository.SaveAsync(eventModel);
        }
    }

    public List<BaseEvent?> GetEvents(Guid aggregateId)
    {
        var eventStream = eventStoreRepository.FindByAggregateId(aggregateId);
        if (eventStream == null || !eventStream.Any())
        {
            throw new AggregateNotFoundException($"Aggregate not found ${aggregateId}");
        }

        return eventStream.OrderBy(x => x.Version).Select(x => x.Event).ToList();
    }
    
    public List<BaseEvent?> GetEventsWithoutThrow(Guid aggregateId)
    {
        var eventStream = eventStoreRepository.FindByAggregateId(aggregateId);
        return eventStream.OrderBy(x => x.Version).Select(x => x.Event).ToList();
    }
}

public class ConcurrencyException : Exception
{
}