using LegalService.Models;

namespace LegalService.Services{
public interface IAuctionRepository
{
    public Task<Auction> GetAuctionById(string auctionId);
    public Task<IEnumerable<Auction>> GetAllAuctions();
    /*
    Task<IEnumerable<Auction>> GetAuctionsByCategory(string category);
    Task<IEnumerable<Auction>> GetAuctionsByStatus(AuctionStatus status);
   
    Task<IEnumerable<Auction>> GetAuctionsByUser(string userId);
    */

}
}
