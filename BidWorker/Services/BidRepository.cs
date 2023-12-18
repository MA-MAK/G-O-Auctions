using System.Threading.Tasks;
using BidWorker.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace BidWorker.Services
{
    public class BidRepository
    {
        private readonly IMongoCollection<Bid> _bids;
        private readonly ILogger<BidRepository> _logger;

        public BidRepository(MongoDBContext dbContext, ILogger<BidRepository> logger)
        {
            _bids = dbContext.Bids;
            _logger = logger;
        }

        public async Task InsertBid(Bid bid)
        {
            _logger.LogInformation($"Bid with ID: {bid.Id} is ready to be posted");

            // Get all existing bids for the same ID
            var existingBids = await _bids.Find(a => a.AuctionId == bid.AuctionId).ToListAsync();

            if (bid.Amount > existingBids.Max(b => b.Amount))
            {
                // If the new bid amount is strictly greater than all existing bids, insert the new bid
                _logger.LogInformation($"Bid with ID is higher: {bid.Id}");
                await _bids.InsertOneAsync(bid);
            }
            else
            {
                // Log or handle the case where a bid with the same ID and a lower or equal amount already exists
                // You may want to throw an exception, log a message, or take other appropriate action
                // For now, let's just log a message
                _logger.LogInformation($"Bid with ID {bid.Id} and a lower or equal amount already exists in the database.");
            }
        }





    }
}

