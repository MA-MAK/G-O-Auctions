using System.Collections.Generic;
using AuctionService.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace AuctionService.Services;

public class AuctionRepository : IAuctionRepository
{
    private readonly IMongoCollection<Auction> _auctions;
    private readonly ILogger<AuctionRepository> _logger;

    public AuctionRepository(MongoDBContext dbContext, ILogger<AuctionRepository> logger)
    {
        _auctions = dbContext.Auctions;
        _logger = logger;
    }

    public Task PostAuction(Auction auction)
    {
        _logger.LogInformation($"count: {_auctions.CountDocuments(a => true)}");
        _logger.LogInformation("AuctionRepository.PostAuction");
        _auctions.InsertOne(auction);
        _logger.LogInformation($"count: {_auctions.CountDocuments(a => true)}");
        _logger.LogInformation("AuctionRepository.PostAuction - Auction inserted");
        return Task.CompletedTask;
    }

    public Task DeleteAuction(Auction auction)
    {
        _auctions.DeleteOneAsync(a => a.Id == auction.Id);
        return Task.CompletedTask;
    }

    /*  public Task<IEnumerable<Auction>> GetAllAuctions()
     {
          var auctions =  _auctions.Find(_ => true).ToListAsync();
          return Task.FromResult<IEnumerable<Auction>>(auctions.Result);
     }

     */
    public Task<Auction> GetAuctionById(string id)
    {
        var auction = _auctions.Find(a => a.Id == id).FirstOrDefaultAsync();
        return Task.FromResult<Auction>(auction.Result);
    }

    public Task UpdateAuction(Auction auction)
    {

        _auctions.ReplaceOneAsync(a => a.Id == auction.Id, auction);
        return Task.CompletedTask;

    }
}
