using System.Collections.Generic;
using ItemService.Models;
using MongoDB.Driver;

namespace ItemService.Services
{
    public class ItemRepository : IItemRepository
    {
        private readonly IMongoCollection<Item> _items;
        private readonly ILogger<ItemRepository> _logger;

        public ItemRepository(MongoDBContext dbContext, ILogger<ItemRepository> logger)
        {
            _items = dbContext.Items;
            _logger = logger;
        }

        public Task<Item> GetItemById(string itemId)
        {
            return Task.FromResult<Item>(_items.Find(a => a.Id == itemId).FirstOrDefault());
        }

        public async Task<IEnumerable<Item>> GetAllItems()
        {
            try
            {
                var allItems = await _items.Find(_ => true).ToListAsync();
                return allItems;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while retrieving all items: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateItem(Item item)
        {
            try
            {
                _logger.LogInformation($"### ItemRepository.UpdateItem - item: {item.Id}");

                // Check if the item exists before attempting to update
                var existingItem = await _items.Find(a => a.Id == item.Id).FirstOrDefaultAsync();

                if (existingItem == null)
                {
                    _logger.LogWarning($"Item with Id {item.Id} not found. Update failed.");
                    return false; // Item not found, update failed
                }

                var filter = Builders<Item>.Filter.Eq(a => a.Id, item.Id);
                var updateDefinition = Builders<Item>.Update
                    .Set(a => a.Title, item.Title)
                    .Set(a => a.AssesmentPrice, item.AssesmentPrice)
                    .Set(a => a.Description, item.Description)
                    .Set(a => a.Year, item.Year)
                    .Set(a => a.Category, item.Category)
                    .Set(a => a.Condition, item.Condition)
                    .Set(a => a.Status, item.Status);

                var result = await _items.UpdateOneAsync(filter, updateDefinition);

                _logger.LogInformation($"### ItemRepository.UpdateItem - result: {result.ModifiedCount}");

                return result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while updating item: {ex.Message}");
                return false;
            }
        }


    }
}