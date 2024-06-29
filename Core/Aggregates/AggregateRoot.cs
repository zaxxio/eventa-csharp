using System.Diagnostics;
using System.Reflection;
using Core.Event;
using Core.Streotype;

namespace Core.Aggregates;

public class AggregateRoot
{
    public Guid AggregateId { get; set; }
    public int Version { get; set; } = -1;
    private readonly List<BaseEvent> _uncommittedChanges = new();

    protected void Apply(BaseEvent baseEvent)
    {
        ApplyEvent(baseEvent, true);
    }

    private void ApplyEvent(BaseEvent baseEvent, bool isNewEvent)
    {
        HandleEvent(baseEvent);
        if (isNewEvent)
        {
            _uncommittedChanges.Add(baseEvent);
        }
        Version++;
    }

    public void MarkChangesAsCommitted()
    {
        _uncommittedChanges.Clear();
    }

    public List<BaseEvent> UncommittedChanges()
    {
        return _uncommittedChanges;
    }

    public void ReplayEvent(List<BaseEvent?> baseEvents)
    {
        foreach (BaseEvent baseEvent in baseEvents)
        {
            ApplyEvent(baseEvent, false);
        }
    }

    private void HandleEvent(BaseEvent baseEvent)
    {
        Type aggregateType = GetType();
        MethodInfo[] methods = aggregateType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        foreach (var method in methods)
        {
            var handlerAttributes = method.GetCustomAttributes(typeof(EventSourcingHandlerAttribute), true);
            if (handlerAttributes.Length > 0)
            {
                var parameters = method.GetParameters();
                if (parameters.Length == 1 && parameters[0].ParameterType == baseEvent.GetType())
                {
                    method.Invoke(this, [baseEvent]);
                }
            }
        }
    }
}