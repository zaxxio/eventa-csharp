using Core.Event;

namespace Core.Repository;

public interface IEventStoreRepository
{
    void SaveAsync(EventModel eventModel);
    List<EventModel> FindByAggregateId(Guid aggregateId);
}