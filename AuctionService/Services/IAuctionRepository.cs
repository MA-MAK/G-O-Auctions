using AuctionService.Models;

namespace AuctionService.Services{
public interface IAuctionRepository
{
    public Task<Auction> GetAuctionById(string auctionId);
    // public Task<IEnumerable<Auction>> GetAllAuctions();
    /*
    Task<IEnumerable<Auction>> GetAuctionsByCategory(string category);
    Task<IEnumerable<Auction>> GetAuctionsByStatus(AuctionStatus status);
   
    Task<IEnumerable<Auction>> GetAuctionsByUser(string userId);
    */
    public Task InsertAuction(Auction auction);
    public Task UpdateAuction(Auction auction);
    public Task DeleteAuction(Auction auction);
}
}
