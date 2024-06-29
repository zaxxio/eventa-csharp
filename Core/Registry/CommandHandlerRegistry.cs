using System.Reflection;
using Core.Streotype;

namespace Core.Registry;

public class CommandHandlerRegistry : HandlerRegistry
{
    private readonly Dictionary<Type, MethodInfo> _commandHandlers;
    private readonly Dictionary<Type, ConstructorInfo> _constructorHandlers;

    public CommandHandlerRegistry()
    {
        _commandHandlers = new Dictionary<Type, MethodInfo>();
        _constructorHandlers = new Dictionary<Type, ConstructorInfo>();
        OnInit();
    }

    private void OnInit()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies(); 

        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                var aggregateAttribute = type.GetCustomAttribute<AggregateAttribute>();
                if (aggregateAttribute != null)
                {
                    var methods = type.GetMethods();
                    foreach (var method in methods)
                    {
                        var commandHandlerAttribute = method.GetCustomAttribute<CommandHandlerAttribute>();
                        if (commandHandlerAttribute != null)
                        {
                            var commandType = method.GetParameters()[0].ParameterType;
                            RegisterHandler(commandType, method);
                        }
                    }

                    var constructors = type.GetConstructors();
                    foreach (var constructor in constructors)
                    {
                        var commandHandlerAttribute = constructor.GetCustomAttribute<CommandHandlerAttribute>();
                        if (commandHandlerAttribute != null)
                        {
                            var commandType = constructor.GetParameters()[0].ParameterType;
                            RegisterConstructorHandler(commandType, constructor);
                        }
                    }
                }
            }
        }
    }

    public void RegisterHandler(Type type, MethodInfo handlerMethod)
    {
        if (!_commandHandlers.TryAdd(type, handlerMethod))
        {
            throw new InvalidOperationException($"Command handler for '{type.Name}' is already registered.");
        }
    }

    public void RegisterConstructorHandler(Type type, ConstructorInfo constructorHandler)
    {
        if (!_constructorHandlers.TryAdd(type, constructorHandler))
        {
            throw new InvalidOperationException($"Command handler for '{type.Name}' is already registered.");
        }
    }

    public MethodInfo GetHandler(Type type)
    {
        _commandHandlers.TryGetValue(type, out var handler);
        return handler;
    }

    public ConstructorInfo? GetConstructorHandler(Type type)
    {
        _constructorHandlers.TryGetValue(type, out var handler);
        return handler;
    }

    public List<MethodInfo> GetHandlers()
    {
        return new List<MethodInfo>(_commandHandlers.Values);
    }

    public List<ConstructorInfo> GetConstructorHandlers()
    {
        return new List<ConstructorInfo>(_constructorHandlers.Values);
    }
}