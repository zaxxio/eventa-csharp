using Core.Event;
using Core.Interceptor;
using Core.Producer;
using Core.Repository;

namespace Core.Extensions;

public class EventaOptionsBuilder
{
    private readonly EventaOptions _options = new EventaOptions();

    public EventaOptionsBuilder EnableLogging(bool enable = true)
    {
        _options.EnableLogging = enable;
        return this;
    }

    public EventaOptionsBuilder EnableValidation(bool enable = true)
    {
        _options.EnableValidation = enable;
        return this;
    }

    public EventaOptionsBuilder UseEventProducer<T>() where T : EventProducer
    {
        _options.EventProducerType = typeof(T);
        return this;
    }

    public EventaOptionsBuilder UseEventStore<T>() where T : IEventStore
    {
        _options.EventStoreType = typeof(T);
        return this;
    }

    public EventaOptionsBuilder UseEventStoreRepository<T>() where T : IEventStoreRepository
    {
        _options.EventStoreRepositoryType = typeof(T);
        return this;
    }

    public EventaOptionsBuilder AddInterceptor<T>() where T : ICommandInterceptor
    {
        _options.InterceptorTypes.Add(typeof(T));
        return this;
    }

    internal EventaOptions Build() => _options;
}