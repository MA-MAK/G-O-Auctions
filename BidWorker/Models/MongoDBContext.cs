using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using BidWorker.Models;
using BidWorker.Services;

namespace BidWorker.Models;

/// <summary>
/// MongoDB database context class.
/// </summary>
public class MongoDBContext
{
    private ILogger<MongoDBContext> _logger;
    public IMongoDatabase _goDatabase { get; set; }

    /// <summary>
    /// Create an instance of the context class.
    /// </summary>
    /// <param name="logger">Global logging facility.</param>
    /// <param name="config">System configuration instance.</param>
    public MongoDBContext(ILogger<MongoDBContext> logger)
    {
        _logger = logger;

        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

        var client = new MongoClient(Environment.GetEnvironmentVariable("connectionString") ?? "mongodb://localhost:27017");
        _goDatabase = client.GetDatabase(Environment.GetEnvironmentVariable("databaseName"));
    }

    public IMongoCollection<Bid> Bids => _goDatabase.GetCollection<Bid>(Environment.GetEnvironmentVariable("collectionName"));
}