
public interface IBidRepository
{
    Task<IEnumerable<Bid>> GetBidsForAuction(string auctionId);
}

