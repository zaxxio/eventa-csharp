namespace Core.Event;

public abstract class BaseEvent : Message.Message
{
    public int Version { get; set; }
    public string? Type { get; set; }
}