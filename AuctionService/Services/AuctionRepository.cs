using System.Collections.Generic;
using AuctionService.Models;

namespace AuctionService.Services{
public class AuctionRepository : IAuctionRepository
{
    private List<Auction> auctions;

    public AuctionRepository()
    {
        auctions = new List<Auction>();
    }

    public Task PostAuction(Auction auction)
    {
        auctions.Add(auction);
        return Task.CompletedTask;
    }

    

    public Task DeleteAuction(Auction auction)
    {
        auctions.Remove(auction);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Auction>> GetAllAuctions()
    {
        return Task.FromResult<IEnumerable<Auction>>(auctions);
    }

    public Task <Auction> GetAuctionById(int id)
    {
        return Task.FromResult(auctions.Find(a => a.Id == id));
    }

    public Task UpdateAuction(Auction auction)
    {
        int index = auctions.FindIndex(a => a.Id == auction.Id);
        if (index != -1)
        {
            auctions[index] = auction;
        }
        return Task.CompletedTask;
    }
}
}

    



