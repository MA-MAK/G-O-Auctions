using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using AuctionService.Models;
using AuctionService.Services;
using NLog;

namespace AuctionService.Models
{

// interface for MongoDBContext:
public interface IMongoDBContext
{
    IMongoCollection<Auction> auctions { get; }

    IMongoDatabase GODatabase { get; set; }

}

}