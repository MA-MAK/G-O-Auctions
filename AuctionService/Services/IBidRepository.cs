
public interface IBidRepository
{
    Task<IEnumerable<Bid>> GetBidsForAuction(int auctionId);
}

