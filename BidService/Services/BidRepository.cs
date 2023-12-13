using System.Collections.Generic;
using BidService.Models;
using MongoDB.Driver;

namespace BidService.Services;

public class BidRepository : IBidRepository
{
    private readonly IMongoCollection<Bid> _bids;
    private readonly ILogger<BidRepository> _logger;

    public BidRepository(MongoDBContext dbContext, ILogger<BidRepository> logger)
    {
        _bids = dbContext.Bids;
        _logger = logger;
    }

    public Task<IEnumerable<Bid>> GetBidsForAuction(string auctionId)
    {
        _logger.LogInformation($"### BidRepository.GetBidsForAuction - auctionId: {auctionId}");
        return Task.FromResult<IEnumerable<Bid>>(_bids.Find(a => a.AuctionId == auctionId).ToList());
    }
}
