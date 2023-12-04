using System.Collections.Generic;
using AuctionService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AuctionService.Services
{
    public class AuctionRepository : IAuctionRepository
    {
        private List<Auction> auctions;
        private readonly MongoDBContext _dbContext;
        private readonly ILogger<AuctionRepository> _logger;
        private readonly IConfiguration _configuration;

        public AuctionRepository(MongoDBContext dbContext, ILogger<AuctionRepository> logger, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            auctions = new List<Auction>();
        }

        public Task PostAuction(Auction auction)
        {
            auctions.Add(auction);
            // Add logic to save to MongoDB using _dbContext if needed
            return Task.CompletedTask;
        }

        public Task DeleteAuction(Auction auction)
        {
            auctions.Remove(auction);
            // Add logic to delete from MongoDB using _dbContext if needed
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Auction>> GetAllAuctions()
        {
            // Add logic to retrieve from MongoDB using _dbContext if needed
            return Task.FromResult<IEnumerable<Auction>>(auctions);
        }

        public Task<Auction> GetAuctionById(int id)
        {
            // Add logic to retrieve from MongoDB using _dbContext if needed
            return Task.FromResult(auctions.Find(a => a.Id == id));
        }

        public Task UpdateAuction(Auction auction)
        {
            int index = auctions.FindIndex(a => a.Id == auction.Id);
            if (index != -1)
            {
                auctions[index] = auction;
                // Add logic to update in MongoDB using _dbContext if needed
            }
            return Task.CompletedTask;
        }
    }
}
