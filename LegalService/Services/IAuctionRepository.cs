using LegalService.Models;

namespace LegalService.Services
{
    public interface IAuctionRepository
    {
        public Task<Auction> GetAuctionById(string auctionId);
        public Task<IEnumerable<Auction>> GetAllAuctions();
    }
}
