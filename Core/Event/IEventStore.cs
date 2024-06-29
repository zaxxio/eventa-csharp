namespace Core.Event;

public interface IEventStore
{
    void SaveEvents(Guid aggregateId, string aggregateType, List<BaseEvent> uncommittedChanges,
        int expectedVersion, bool constructor);

    List<BaseEvent?> GetEvents(Guid aggregateId);
    List<BaseEvent?> GetEventsWithoutThrow(Guid aggregateId);
}