using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using SaleService.Models;
using SaleService.Services;
using NLog;

namespace SaleService.Models;

/// <summary>
/// MongoDB database context class.
/// </summary>
public class MongoDBContext 
{
    private ILogger<MongoDBContext> _logger;
    public IMongoDatabase _goDatabase { get; set; }
    public IMongoCollection<Sale> sales { get; set; }

    /// <summary>
    /// Create an instance of the context class.
    /// </summary>
    /// <param name="logger">Global logging facility.</param>
    /// <param name="config">System configuration instance.</param>
    public MongoDBContext(ILogger<MongoDBContext> logger)
    {
        _logger = logger;
        
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        var connectionString = Environment.GetEnvironmentVariable("connectionString");
        _logger.LogInformation($"### MongoDBContext.MongoDBContext - connectionString: {connectionString}");
        var client = new MongoClient(Environment.GetEnvironmentVariable("connectionString"));
        _goDatabase = client.GetDatabase(Environment.GetEnvironmentVariable("databaseName"));
    }

    public IMongoCollection<Sale> Sales => _goDatabase.GetCollection<Sale>(Environment.GetEnvironmentVariable("collectionName"));
}
