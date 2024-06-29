using Core.Aggregates;
using Core.Config;
using Core.Dispatcher;
using Core.Event;
using Core.Extensions;
using Core.Interceptor;
using Core.Producer;
using Core.Registry;
using Core.Repository;
using Web.Aggregate;
using Web.Interceptor;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddEventa(optionsBuilder =>
{
    // optionsBuilder
    // .EnableLogging()
    // .EnableValidation()
    // // .UseEventProducer<CustomEventProducer>()
    // // .UseEventStore<CustomEventStore>()
    // .UseEventStoreRepository<CustomEventStoreRepository>()
    // .AddInterceptor<CustomInterceptor1>()
    // .AddInterceptor<CustomInterceptor2>();
});

builder.Services.AddSingleton<CommandHandlerRegistry>();
builder.Services.AddSingleton<EventProducer>();
builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();
builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();
builder.Services.AddTransient<ProductAggregate>();
builder.Services.AddSingleton<IEventStoreRepository, MongoDbEventStoreRepository>();
builder.Services.AddSingleton<IEventStore, MongoEventStore>();
builder.Services.AddTransient<ICommandInterceptor, ProductCommandInterceptor>();
builder.Services.AddSingleton<CommandInterceptorRegisterer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapControllers();

// app.UseHttpsRedirection();
app.Run();