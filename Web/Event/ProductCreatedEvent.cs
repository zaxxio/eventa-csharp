using Core.Event;
using Core.Streotype;

namespace Web.Event;

public class ProductCreatedEvent : BaseEvent
{
    [RoutingKey]
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }
}