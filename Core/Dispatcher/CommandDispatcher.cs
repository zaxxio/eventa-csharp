using System.Diagnostics;
using System.Reflection;
using Core.Aggregates;
using Core.Command;
using Core.Event;
using Core.Registry;
using Core.Streotype;

namespace Core.Dispatcher;

public interface ICommandDispatcher
{
    void Dispatch<TCommand>(TCommand command) where TCommand : BaseCommand;
}

public class CommandDispatcher(
    IServiceProvider serviceProvider,
    CommandHandlerRegistry commandHandlerRegistry,
    IEventStore eventStore,
    CommandInterceptorRegisterer commandInterceptorRegisterer)
    : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider =
        serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    private readonly CommandHandlerRegistry _commandHandlerRegistry =
        commandHandlerRegistry ?? throw new ArgumentNullException(nameof(commandHandlerRegistry));

    private readonly IEventStore _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));

    private readonly CommandInterceptorRegisterer _commandInterceptorRegisterer = commandInterceptorRegisterer ??
        throw new ArgumentNullException(
            nameof(commandInterceptorRegisterer));


    public void Dispatch<TCommand>(TCommand command) where TCommand : BaseCommand
    {
        if (command == null) throw new ArgumentNullException(nameof(command));

        commandInterceptorRegisterer.PreHandle(command);

        Guid routingKey = ExtractRoutingKey(command);
        command.MessageId = routingKey;

        try
        {
            var constructorHandler = _commandHandlerRegistry.GetConstructorHandler(command.GetType());
            if (constructorHandler != null)
            {
                var declaringType = constructorHandler.DeclaringType;
                if (declaringType != null)
                {
                    if (eventStore.GetEventsWithoutThrow(routingKey).Count > 0)
                    {
                        throw new Exception($"Aggregate {routingKey} is already exists");
                    }

                    AggregateRoot? aggregateRoot = (AggregateRoot)_serviceProvider.GetService(declaringType)!;
                    constructorHandler.Invoke(aggregateRoot, new object[] { command });
                    var uncommittedChanges = aggregateRoot.UncommittedChanges();
                    _eventStore.SaveEvents(routingKey, declaringType.Name, uncommittedChanges, -1, true);
                    aggregateRoot.MarkChangesAsCommitted();
                    commandInterceptorRegisterer.PostHandle(command);
                    return;
                }
            }

            var commandHandler = _commandHandlerRegistry.GetHandler(command.GetType());
            {
                var aggregate =
                    (AggregateRoot)_serviceProvider.GetService(commandHandler.DeclaringType ??
                                                               throw new InvalidOperationException())!;

                Debug.WriteLine(aggregate);
                var baseEvents = eventStore.GetEvents(routingKey);
                aggregate.ReplayEvent(baseEvents);
                Debug.WriteLine(aggregate);
                commandHandler.Invoke(aggregate, [command]);
                var uncommittedChanges = aggregate.UncommittedChanges();
                _eventStore.SaveEvents(routingKey, aggregate.GetType().Name, uncommittedChanges, aggregate.Version,
                    false);
                aggregate.MarkChangesAsCommitted();
                commandInterceptorRegisterer.PostHandle(command);
            }
        }
        catch (Exception ex)
        {
            // Log the exception
            Debug.WriteLine($"Error: {ex.Message}");
            // Rethrow the exception to be handled by the global error handler
            throw new InvalidOperationException("An error occurred while dispatching the command.", ex);
        }
        finally
        {
            commandInterceptorRegisterer.PostHandle(command);
        }
    }

    public Guid ExtractRoutingKey<TCommand>(TCommand command)
    {
        var commandType = typeof(TCommand);
        var properties = commandType.GetProperties();
        var routingKeyProp = properties.FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(RoutingKeyAttribute)));
        if (routingKeyProp != null)
        {
            var routingKeyAttr = routingKeyProp.GetCustomAttribute<RoutingKeyAttribute>();
            if (routingKeyAttr != null && routingKeyProp.PropertyType == typeof(Guid))
            {
                var value = (Guid)routingKeyProp.GetValue(command);
                return value;
            }

            throw new ArgumentException(
                $"Property '{routingKeyProp.Name}' in '{commandType.Name}' is not of type Guid.");
        }

        throw new ArgumentException($"No property marked with [RoutingKey] attribute found in '{commandType.Name}'.");
    }
}