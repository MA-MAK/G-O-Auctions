using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using CustomerService.Models;
using CustomerService.Services;

namespace CustomerService.Models;

/// <summary>
/// MongoDB database context class.
/// </summary>
public class MongoDBContext 
{
    private ILogger<MongoDBContext> _logger;
    private IMongoDatabase _goDatabase { get; set; }
    private IMongoCollection<Customer> _customers { get; set; }

    /// <summary>
    /// Create an instance of the context class.
    /// </summary>
    /// <param name="logger">Global logging facility.</param>
    public MongoDBContext(ILogger<MongoDBContext> logger)
    {
        _logger = logger;
        
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

        var client = new MongoClient(Environment.GetEnvironmentVariable("MongoDBConnection"));
        _goDatabase = client.GetDatabase(Environment.GetEnvironmentVariable("DatabaseName"));
        _customers = _goDatabase.GetCollection<Customer>(Environment.GetEnvironmentVariable("CustomerCollection"));
    }

    public IMongoCollection<Customer> Customers => _goDatabase.GetCollection<Customer>("Customers");

}

