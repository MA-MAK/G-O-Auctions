using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Formatting;

namespace AuctionService.Services
{
    public class ItemRepository : IItemRepository
    {
        private readonly HttpClient _httpClient;

        public ItemRepository()//HttpClient httpClient)
        {
            //_httpClient = httpClient;
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
                throw new Exception($"Failed to get item with ID {itemId}. Status code: {response.StatusCode}");
            }
        }
    }
}


