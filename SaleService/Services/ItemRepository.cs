using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using SaleService.Models;

namespace SaleService.Services
{
    public class ItemRepository : IItemRepository
    {
        private readonly HttpClient _httpClient;

        public ItemRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Item> GetItemById(int itemId)
        {
            // Make a GET request to the API endpoint with the item ID
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/items/{itemId}");

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response content to an Item object
                Item item = await response.Content.ReadAsAsync<Item>();
                return item;
            }
            else
            {
                // Handle the error response
                throw new Exception(
                    $"Failed to get item with ID {itemId}. Status code: {response.StatusCode}"
                );
            }
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
                    _logger.LogInformation(
                        $"### ItemRepository.GetItemById - response: {response}"
                    );
                    // Deserialize the response content to an Item object
                    //Item item = await response.Content.ReadAsAsync<Item>();

                    string jsonString = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation(
                        $"### ItemRepository.GetItemById - jsonString: {jsonString}"
                    );
                    //Item item = JsonSerializer.Deserialize<Item>(jsonString);
                    Item item = JsonSerializer.Deserialize<Item>(
                        jsonString,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                    _logger.LogInformation($"### ItemRepository.GetItemById - item: {item.Id}");
                    return item;
                }
                else
                {
                    _logger.LogError(
                        $"### Failed to get item with ID {itemId}. Status code: {response.StatusCode}"
                    );
                    // Handle the error response
                    throw new Exception(
                        $"Failed to get item with ID {itemId}. Status code: {response.StatusCode}"
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    $"### Error in GetItemById for item ID {itemId}: {ex.Message}"
                );
                // Log and handle the exception appropriately
                throw new Exception($"Error in GetItemById: {ex.Message}", ex);
            }
        }
    }
}
