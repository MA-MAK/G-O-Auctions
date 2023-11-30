
public interface IItemRepository
{
    Task<Item> GetItemById(int itemId);
}
