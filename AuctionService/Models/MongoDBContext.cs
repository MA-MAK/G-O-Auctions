using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using AuctionService.Models;
using AuctionService.Services;
using NLog;

namespace AuctionService.Models
{

/// <summary>
/// MongoDB database context class.
/// </summary>
public class MongoDBContext: IMongoDBContext
{
    private NLog.Logger _logger;
    private IConfiguration _config;
    public virtual IMongoDatabase GODatabase { get; set; }
    public virtual IMongoCollection<Auction> auctions { get; set; }



    /// <summary>
    /// Create an instance of the context class.
    /// </summary>
    /// <param name="logger">Global logging facility.</param>
    /// <param name="config">System configuration instance.</param>
    public MongoDBContext(NLog.Logger logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
        
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

        var client = new MongoClient(_config["MongoDBSettings:MongoConnectionString"]);
        GODatabase = client.GetDatabase(_config["MongoDBSettings:DatabaseName"]);
        auctions = GODatabase.GetCollection<Auction>(_config["MongoDBSettings:AuctionCollection"]);

        _logger.Debug($"Connected to database {_config["MongoDBSettings:DatabaseName"]}");
        _logger.Debug($"Using collection {_config["MongoDBSettings:AuctionCollection"]}");

    }

    public IMongoCollection<Auction> Auctions => GODatabase.GetCollection<Auction>("Auctions");

}

}
