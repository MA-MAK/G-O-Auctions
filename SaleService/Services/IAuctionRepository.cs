using SaleService.Models;
public interface IAuctionRepository
{
    Task<Auction> GetAuctionById(string auctionId);
}
