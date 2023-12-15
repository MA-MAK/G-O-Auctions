using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using AuctionService.Models;
using NLog.Fluent;
using System.Text.Json;

namespace AuctionService.Services
{
    public class ItemRepository : IItemRepository
    {
        private List<Item> itemsReadyForAuction;
        private readonly HttpClient _httpClient;
        private readonly ILogger<ItemRepository> _logger;

        public ItemRepository(HttpClient httpClient, ILogger<ItemRepository> logger)
        {
            itemsReadyForAuction = new List<Item>();
            _httpClient = httpClient;
            _logger = logger;

            _logger.LogInformation($"### ItemRepository - _httpClient: {_httpClient.BaseAddress}");
        }

        public async Task<Item> GetItemById(string itemId)
        {
            _logger.LogInformation($"### ItemRepository.GetItemById - itemId: {itemId}");

            try
            {
            // Make a GET request to the API endpoint with the item ID
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/item/{itemId}");

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"### ItemRepository.GetItemById - response: {response}");
                // Deserialize the response content to an Item object
                //Item item = await response.Content.ReadAsAsync<Item>();

                string jsonString = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"### ItemRepository.GetItemById - jsonString: {jsonString}");
                //Item item = JsonSerializer.Deserialize<Item>(jsonString);
                Item item = JsonSerializer.Deserialize<Item>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                _logger.LogInformation($"### ItemRepository.GetItemById - item: {item.Id}");
                return item;
            }
            else
            {
                _logger.LogError($"### Failed to get item with ID {itemId}. Status code: {response.StatusCode}");
                // Handle the error response
                throw new Exception($"Failed to get item with ID {itemId}. Status code: {response.StatusCode}");
            }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"### Error in GetItemById for item ID {itemId}: {ex.Message}");
                // Log and handle the exception appropriately
                throw new Exception($"Error in GetItemById: {ex.Message}", ex);
            }
        }


        // Get all Items ready for auction
         public async Task<IEnumerable<Item>> GetAllItemsReadyForAuction()
        {

            try
            {
                // Make a GET request to the ItemService API endpoint to get all items
                HttpResponseMessage response = await _httpClient.GetAsync("/api/items");
                _logger.LogInformation($"### ItemRepository - _httpClient: {_httpClient.BaseAddress}");
                _logger.LogInformation($"### ItemRepository.GetAllItemsReadyForAuction - response: {response}");
                
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response content to a list of Item objects
                    string jsonString = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"### ItemRepository.GetAllItemsReadyForAuction - jsonString: {jsonString}");
                    var allItems = JsonSerializer.Deserialize<List<Item>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    // Filter the items to get only the "ReadyForAuction" ones
                    var itemsReadyForAuction = allItems.Where(i => i.Status == Status.ReadyForAuction);
                    return itemsReadyForAuction;
                }   
                else
                {
                    _logger.LogError($"### Failed to get all ready items. Status code: {response.StatusCode}");
                    throw new Exception($"Failed to get items. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"### Error in GetAllItemsReadyForAuction: {ex.Message}");
                throw new Exception($"Error in GetAllItemsReadyForAuction: {ex.Message}", ex);
            }

        }
    }
}