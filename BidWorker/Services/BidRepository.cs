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
            _logger.LogInformation($"Bid with ID: {bid.Id} is ready to be posted");

            // Get all existing bids for the same ID
            var existingBids = await _bids.Find(b => b.Id == bid.Id).ToListAsync();

            // Check if the new bid amount is strictly greater than all existing bids
            if (existingBids.All(existingBid => bid.Amount > existingBid.Amount))
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

