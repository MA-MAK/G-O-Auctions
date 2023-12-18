using BidService.Models;

namespace BidService.Services;
public interface IBidRepository
{
    Task<IEnumerable<Bid>> GetBidsForAuction(string auctionId);
    Task<bool> PostBid(Bid newBid);
}