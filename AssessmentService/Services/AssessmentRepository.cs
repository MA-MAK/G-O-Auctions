using System.Collections.Generic;
using System.Threading.Tasks;
using AssessmentService.Models;
using System.Net.Http.Formatting;
using AssessmentService.Models;
using NLog.Fluent;
using System.Text.Json;
using System.Net.Http;
using System.Text;

namespace AssessmentService.Services
{

    public class AssessmentRepository : IAssessmentRepository
    {

        private List<Item> registeredItems;
        private readonly HttpClient _httpClient;
        private readonly ILogger<AssessmentRepository> _logger;


        public AssessmentRepository(HttpClient httpClient, ILogger<AssessmentRepository> logger)
        {
            registeredItems = new List<Item>();
            _httpClient = httpClient;
            _logger = logger;

            _logger.LogInformation($"### ItemRepository - _httpClient: {_httpClient.BaseAddress}");

        }

        // Get method to get item by ID via ItemService
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

        // Get method to get all items marked as registred via ItemService
        // Get method to get all items marked as registered via ItemService
        public async Task<IEnumerable<Item>> GetAllRegistredItems()
        {
            try
            {
                // Make a GET request to the ItemService API endpoint to get all items
                HttpResponseMessage response = await _httpClient.GetAsync("/api/item/all");

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response content to a list of Item objects
                    string jsonString = await response.Content.ReadAsStringAsync();
                    var allItems = JsonSerializer.Deserialize<List<Item>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    // Filter the items to get only the registered ones
                    var registeredItems = allItems.Where(i => i.Status == Status.Registered);

                    return registeredItems;
                }
                else
                {
                    _logger.LogError($"### Failed to get all items. Status code: {response.StatusCode}");
                    throw new Exception($"Failed to get all items. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"### Error in GetAllRegistredItems: {ex.Message}");
                throw new Exception($"Error in GetAllRegistredItems: {ex.Message}", ex);
            }
        }



        // Put method to update item on certain attributes via ItemService
        public async Task UpdateItem(Item item)
        {
            try
            {
                // Make a GET request to the ItemService API endpoint to get the item by ID
                /* HttpResponseMessage getItemResponse = await _httpClient.GetAsync($"/api/item/{item.Id}");

                 if (!getItemResponse.IsSuccessStatusCode)
                 {
                     _logger.LogError($"### Failed to get item with ID {item.Id}. Status code: {getItemResponse.StatusCode}");
                     throw new Exception($"Failed to get item with ID {item.Id}. Status code: {getItemResponse.StatusCode}");
                 }

                 // Deserialize the response content to an Item object
                 string getItemJsonString = await getItemResponse.Content.ReadAsStringAsync();
                 var existingItem = JsonSerializer.Deserialize<Item>(getItemJsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
 */
                // Set Status to ReadyForAuction or NotSellable
                if (item.Status != 0)
                {
                    item.Status = (Status)item.Status;
                }

                item.Title = item.Title ?? item.Title;

                // Make a PUT request to update the item in the ItemService
                string updatedItemJsonString = JsonSerializer.Serialize(item);
                StringContent content = new StringContent(updatedItemJsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage updateItemResponse = await _httpClient.PutAsync($"/api/item/", content);

                if (!updateItemResponse.IsSuccessStatusCode)
                {
                    _logger.LogError($"### Failed to update item with ID {item.Id}. Status code: {updateItemResponse.StatusCode}");
                    throw new Exception($"Failed to update item with ID {item.Id}. Status code: {updateItemResponse.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"### Error in UpdateItem: {ex.Message}");
                throw new Exception($"Error in UpdateItem: {ex.Message}", ex);
            }
        }

    }
}
