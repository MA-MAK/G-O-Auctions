using AuctionService.Models;
public interface IItemRepository
{
    Task<Item> GetItemById(string itemId);

    public Task<IEnumerable<Item>> GetAllItemsReadyForAuction();
}
