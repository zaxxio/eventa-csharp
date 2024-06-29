using Core.Event;

namespace Core.Producer;

public interface IEventProducer
{
    Task ProduceAsync<T>(T baseEvent) where T : BaseEvent;
}