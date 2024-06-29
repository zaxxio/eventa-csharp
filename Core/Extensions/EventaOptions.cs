using Core.Event;
using Core.Producer;
using Core.Repository;

namespace Core.Extensions;

public class EventaOptions
{
    public bool EnableLogging { get; set; }
    public bool EnableValidation { get; set; }

    public Type EventProducerType { get; set; } = typeof(EventProducer);
    public Type EventStoreType { get; set; } = typeof(MongoEventStore);
    public Type EventStoreRepositoryType { get; set; } = typeof(MongoDbEventStoreRepository);
    public List<Type> InterceptorTypes { get; } = new List<Type>();
}
