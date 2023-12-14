using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using SaleService.Models;
using SaleService.Services;
using NLog;

namespace SaleService.Models
{

/// <summary>
/// MongoDB database context class.
/// </summary>
public class MongoDBContext 
{
    private ILogger<MongoDBContext> _logger;
    private IConfiguration _config;
    public IMongoDatabase GODatabase { get; set; }
    public IMongoCollection<Sale> sales { get; set; }

    /// <summary>
    /// Create an instance of the context class.
    /// </summary>
    /// <param name="logger">Global logging facility.</param>
    /// <param name="config">System configuration instance.</param>
    public MongoDBContext(ILogger<MongoDBContext> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
        
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

        var client = new MongoClient(_config["MongoDBSettings:MongoConnectionString"]);
        GODatabase = client.GetDatabase(_config["MongoDBSettings:DatabaseName"]);
        sales = GODatabase.GetCollection<Sale>(_config["MongoDBSettings:SaleCollection"]);

        //_logger.Debug($"Connected to database {_config["MongoDBSettings:DatabaseName"]}");
        //_logger.Debug($"Using collection {_config["MongoDBSettings:AuctionCollection"]}");

    }

    public IMongoCollection<Sale> Sales => GODatabase.GetCollection<Sale>("Sales");

}

}
