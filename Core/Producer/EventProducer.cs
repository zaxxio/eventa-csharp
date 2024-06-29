using System.Text.Json;
using Confluent.Kafka;
using Core.Event;
using Microsoft.Extensions.Options;

namespace Core.Producer;

public class EventProducer(IOptions<ProducerConfig> options) : IEventProducer
{
    private readonly ProducerConfig _producerConfig = options.Value;

    public async Task ProduceAsync<T>(T baseEvent) where T : BaseEvent
    {
        var producer = new ProducerBuilder<string, string>(_producerConfig)
            .SetKeySerializer(Serializers.Utf8)
            .SetValueSerializer(Serializers.Utf8)
            .Build();


        var message = new Message<string, string>()
        {
            Key = Guid.NewGuid().ToString(),
            Value = JsonSerializer.Serialize(baseEvent, baseEvent.GetType())
        };

        var result = await producer.ProduceAsync("BaseEvent", message);
        if (result.Status == PersistenceStatus.NotPersisted)
        {
            throw new Exception($"Failed to send message {baseEvent.GetType().ToString()}");
        }
    }
}