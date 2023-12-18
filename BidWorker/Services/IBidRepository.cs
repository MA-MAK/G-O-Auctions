using BidWorker.Models;

namespace BidWorker.Services;
public interface IBidRepository
{
    Task InsertBid(Bid bid);
}