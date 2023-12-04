using ItemService.Models;

namespace ItemService.Services
{
    public interface IItemRepository
    {
        public Task<Item> GetItemForAuction(int auctionId);
    }
}
