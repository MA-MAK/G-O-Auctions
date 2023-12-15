using System.Collections.Generic;
using BidService.Models;
using MongoDB.Driver;
using BidService.Services;

namespace BidService.Services;

public class BidRepository : IBidRepository
{
    private readonly IMongoCollection<Bid> _bids;
    private readonly ILogger<BidRepository> _logger;
    private readonly ICustomerRepository _customerRepository;

    public BidRepository(MongoDBContext dbContext, ILogger<BidRepository> logger, ICustomerRepository customerRepository)
    {
        _bids = dbContext.Bids;
        _logger = logger;
        _customerRepository = customerRepository;
    }

    public Task<IEnumerable<Bid>> GetBidsForAuction(string auctionId)
    {
        _logger.LogInformation($"### BidRepository.GetBidsForAuction - auctionId: {auctionId}");
        return Task.FromResult<IEnumerable<Bid>>(_bids.Find(a => a.AuctionId == auctionId).ToList());
    }

    public async Task<bool> PostBid(Bid newBid)
    {
        try
        {
            _logger.LogInformation($"### BidRepository.PostBid - auctionId: {newBid.AuctionId}");

            // Check if the new bid amount is higher than existing bids for the same auction
            var existingBids = await _bids.Find(a => a.AuctionId == newBid.AuctionId).ToListAsync();

            if (newBid.Amount < existingBids.Max(b => b.Amount))
            {
                _logger.LogWarning("Bid amount must be higher than existing bids for the same auction.");
                return false; // Bid amount is not higher, post failed
            }

            // Check if the customer with the specified ID exists
            
            var existingCustomer = await _customerRepository.GetCustomerById(newBid.Customer.Id);

            if (existingCustomer == null)
            {
                _logger.LogWarning($"Customer with ID {newBid.Customer.Id} does not exist.");
                return false; // Customer does not exist, post failed
            }


            // Insert the new bid
            await _bids.InsertOneAsync(newBid);

            return true; // Bid posted successfully
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred while posting bid: {ex.Message}");
            return false;
        }
    }

}


