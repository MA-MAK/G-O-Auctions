using System.Collections.Generic;
using System.Threading.Tasks;
using AssessmentService.Models;
using System.Net.Http.Formatting;
using AssessmentService.Models;
using NLog.Fluent;
using System.Text.Json;
using System.Net.Http;

namespace AssessmentService.Services
{
  
    public class AssessmentRepository : IAssessmentRepository
    {

        private List<Item> items;

        private readonly HttpClient _httpClient;
        private readonly ILogger<AssessmentRepository> _logger;

        

        public AssessmentRepository(HttpClient httpClient, ILogger<AssessmentRepository> logger)
        {
            items = new List<Item>();
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

        public Task<IEnumerable<Item>> GetAllRegistredItems()
        {
            // Insert logic here to get all items from ItemService
            return Task.FromResult<IEnumerable<Item>>(items); 
            
        }

        public Task UpdateItem(Item item)
        {
            // Insert logic here to update an item via ItemService
            items.Add(item);
            return Task.CompletedTask;
        }
    }
}
