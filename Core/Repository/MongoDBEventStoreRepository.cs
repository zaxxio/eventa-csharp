using Core.Config;
using Core.Event;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Core.Repository;

public class MongoDbEventStoreRepository : IEventStoreRepository
{
    private readonly IMongoCollection<EventModel> _eventStoreCollection;

    public MongoDbEventStoreRepository(IOptions<MongoDbConfig> options)
    {
        var config = options.Value;

        var mongoClientSettings = MongoClientSettings.FromConnectionString(config.ConnectionString);
        
        if (!string.IsNullOrEmpty(config.Username) && !string.IsNullOrEmpty(config.Password))
        {
            mongoClientSettings.Credential = MongoCredential.CreateCredential(
                config.AuthenticationDatabase,
                config.Username,
                config.Password
            );
        }

        var mongoClient = new MongoClient(mongoClientSettings);
        var mongoDatabase = mongoClient.GetDatabase(config.Database);
        _eventStoreCollection = mongoDatabase.GetCollection<EventModel>(config.Collection);
    }

    public void SaveAsync(EventModel eventModel)
    {
        _eventStoreCollection.InsertOne(eventModel);
    }

    public List<EventModel> FindByAggregateId(Guid aggregateId)
    {
        return _eventStoreCollection.Find(x => x.AggregateIdentifier == aggregateId).ToList();
    }
}