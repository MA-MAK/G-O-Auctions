using System.Collections.Generic;
using ItemService.Models;

namespace ItemService.Services
{
    public class ItemRepository : IItemRepository
    {
        private List<Item> items;

        public ItemRepository()
        {
            items = new List<Item>();
        }

        public Task<Item> GetItemForAuction(int auctionId)
        {
            return Task.FromResult<Item>(items.Where(b => b.AuctionId == auctionId).FirstOrDefault());
        }

    }
}