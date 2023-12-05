using System.Collections.Generic;
using AuctionService.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace AuctionService.Services;

public class AuctionRepository : IAuctionRepository
{
    private readonly IMongoCollection<Auction> _auctions;
    private readonly ILogger<AuctionRepository> _logger;
    private readonly IConfiguration _config;

    public AuctionRepository(IMongoDBContext dbContext, ILogger<AuctionRepository> logger, IConfiguration config)
    {
        _auctions = dbContext.auctions;
        _logger = logger;
        _config = config;
    }

    public Task InsertAuction(Auction auction)
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
          var auctions =  auctions.Find( => true).ToListAsync();
          return Task.FromResult<IEnumerable<Auction>>(auctions.Result);
     }

     */
    public Task<IEnumerable<Auction>> GetAllAuctions()
    {
        var auctions = _auctions.Find(a => true).ToListAsync();
        return Task.FromResult<IEnumerable<Auction>>(auctions.Result);

    }


    public Task<Auction> GetAuctionById(string auctionId)
    {
        var auction = _auctions.Find(a => a.Id == auctionId).FirstOrDefaultAsync();
        return Task.FromResult<Auction>(auction.Result);
    }

    public Task UpdateAuction(Auction auction)
    {

        _auctions.ReplaceOneAsync(a => a.Id == auction.Id, auction);
        return Task.CompletedTask;

    }
}