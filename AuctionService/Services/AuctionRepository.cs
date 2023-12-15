using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionService.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace AuctionService.Services
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly IMongoCollection<Auction> _auctions;
        private readonly ILogger<AuctionRepository> _logger;

        public AuctionRepository(MongoDBContext dbContext, ILogger<AuctionRepository> logger)
        {
            _auctions = dbContext.Auctions;
            _logger = logger;
        }

        public async Task PostAuction(Auction auction)
        {
            try
            {
                _logger.LogInformation($"AuctionRepository.PostAuction - Start");

                if (auction == null)
                {
                    _logger.LogError("AuctionRepository.PostAuction - Invalid auction provided");
                    throw new ArgumentException("Invalid auction provided");
                }

                _logger.LogInformation($"Count before insert: {_auctions.CountDocuments(a => true)}");

                await _auctions.InsertOneAsync(auction);

                _logger.LogInformation($"Count after insert: {_auctions.CountDocuments(a => true)}");
                _logger.LogInformation("AuctionRepository.PostAuction - Auction inserted");
            }
            catch (Exception ex)
            {
                _logger.LogError($"AuctionRepository.PostAuction - Error: {ex.Message}", ex);
                throw;
            }
        }

        public async Task DeleteAuction(Auction auction)
        {
            try
            {
                _logger.LogInformation("AuctionRepository.DeleteAuction - Start");

                if (auction == null)
                {
                    _logger.LogError("AuctionRepository.DeleteAuction - Invalid auction provided");
                    throw new ArgumentException("Invalid auction provided");
                }

                await _auctions.DeleteOneAsync(a => a.Id == auction.Id);

                _logger.LogInformation("AuctionRepository.DeleteAuction - Auction deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError($"AuctionRepository.DeleteAuction - Error: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<IEnumerable<Auction>> GetAllAuctions()
        {
            try
            {
                _logger.LogInformation("AuctionRepository.GetAllAuctions - Start");

                var auctions = await _auctions.Find(_ => true).ToListAsync();

                _logger.LogInformation("AuctionRepository.GetAllAuctions - Auctions retrieved");

                return auctions;
            }
            catch (Exception ex)
            {
                _logger.LogError($"AuctionRepository.GetAllAuctions - Error: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<Auction> GetAuctionById(string id)
        {
            try
            {
                _logger.LogInformation($"AuctionRepository.GetAuctionById - Start, id: {id}");

                if (string.IsNullOrEmpty(id))
                {
                    _logger.LogError("AuctionRepository.GetAuctionById - Invalid id provided");
                    throw new ArgumentException("Invalid id provided");
                }

                var auction = await _auctions.Find(a => a.Id == id).FirstOrDefaultAsync();

                if (auction == null)
                {
                    _logger.LogInformation($"AuctionRepository.GetAuctionById - Auction not found for id: {id}");
                }
                else
                {
                    _logger.LogInformation($"AuctionRepository.GetAuctionById - Auction found, Title: {auction.Title}");
                }

                return auction;
            }
            catch (Exception ex)
            {
                _logger.LogError($"AuctionRepository.GetAuctionById - Error: {ex.Message}", ex);
                throw;
            }
        }

        public async Task UpdateAuction(Auction auction)
        {
            try
            {
                _logger.LogInformation($"AuctionRepository.UpdateAuction - Start, id: {auction?.Id}");

                if (auction == null)
                {
                    _logger.LogError("AuctionRepository.UpdateAuction - Invalid auction provided");
                    throw new ArgumentException("Invalid auction provided");
                }

                await _auctions.ReplaceOneAsync(a => a.Id == auction.Id, auction);

                _logger.LogInformation("AuctionRepository.UpdateAuction - Auction updated");
            }
            catch (Exception ex)
            {
                _logger.LogError($"AuctionRepository.UpdateAuction - Error: {ex.Message}", ex);
                throw;
            }
        }
    }
}