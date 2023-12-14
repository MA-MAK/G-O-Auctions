using System.Collections.Generic;
using System.Threading.Tasks;
using BidWorker.Models;

namespace BidWorker.Services
{
    public interface IBidRepository
    {
        Task<IEnumerable<Bid>> GetBidsForAuction(string auctionId);

        Task<bool> PostBid(Bid newBid);
    }
}
