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

/*
        public async Task<bool> InsertBid(Bid newBid)
        {
            try
            {
                _logger.LogInformation($"### BidRepository.insertBid - auctionId: {newBid.AuctionId}");

                // Check if the new bid amount is higher than existing bids for the same auction
                var existingBids = await _bids.Find(a => a.AuctionId == newBid.AuctionId).ToListAsync();

                if (existingBids.Any() && newBid.Amount <= existingBids.Max(b => b.Amount))
                {
                    _logger.LogWarning("Bid amount must be higher than existing bids for the same auction.");
                    return false; // Bid amount is not higher, insert failed
                }

                // Check if the customer with the specified ID exists

                var existingCustomer = await _customerRepository.GetCustomerById(newBid.CustomerId);

                if (existingCustomer == null)
                {
                    _logger.LogWarning($"Customer with ID {newBid.CustomerId} does not exist.");
                    return false; // Customer does not exist, insert failed
                }


                // Insert the new bid
                await _bids.InsertOneAsync(newBid);

                return true; // Bid inserted successfully
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while inserting bid: {ex.Message}");
                return false;
            }
        }*/
        public async Task InsertBid(Bid bid)
        {
            await _bids.InsertOneAsync(bid);
        }
    }
}

