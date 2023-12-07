using System.Threading.Tasks;
using ItemService.Models;

namespace ItemService.Services
{
    public interface IItemRepository
    {
        Task<Item> GetItemById(string itemId);
        Task<IEnumerable<Item>> GetAllItems();
        Task<bool> UpdateItem(Item item);
    }
}


