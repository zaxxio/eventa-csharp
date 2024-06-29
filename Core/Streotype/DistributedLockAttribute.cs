namespace Core.Streotype;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class DistributedLockAttribute(string resource, int timeoutSeconds) : Attribute
{
    public string Resource { get; } = resource;
    public int TimeoutSeconds { get; } = timeoutSeconds;
}