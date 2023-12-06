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
    }
}