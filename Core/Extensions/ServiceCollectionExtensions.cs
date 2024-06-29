using Core.Command;
using Core.Dispatcher;
using Core.Event;
using Core.Interceptor;
using Core.Producer;
using Core.Registry;
using Core.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventa(this IServiceCollection services, Action<EventaOptionsBuilder> configure)
    {
        var builder = new EventaOptionsBuilder();
        configure(builder);
        var options = builder.Build();
        
        services.AddSingleton(options);

        services.AddSingleton<CommandInterceptorRegisterer>();

        // Register event-specific services
        services.AddSingleton(typeof(EventProducer), options.EventProducerType);
        services.AddSingleton(typeof(IEventStore), options.EventStoreType);
        services.AddSingleton(typeof(IEventStoreRepository), options.EventStoreRepositoryType);
        
        services.AddSingleton<CommandDispatcher>();
        
        // // Register custom interceptors
        // foreach (var interceptorType in options.InterceptorTypes)
        // {
        //     services.AddTransient(typeof(ICommandInterceptor), interceptorType);
        // }

        return services;
    }

}