namespace Core.Aggregates;

public class AggregateNotFoundException(string message) : Exception(message);