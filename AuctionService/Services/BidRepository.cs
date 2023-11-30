using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Formatting;

namespace AuctionService.Services
{
    public class BidRepository : IBidRepository
    {
        private readonly HttpClient _httpClient;

        public BidRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Bid>> GetBidsForAuction(int auctionId)
        {
            // Make a GET request to the API endpoint with the item ID
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/items/{auctionId}");

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response content to an Item object
                return await response.Content.ReadAsAsync<IEnumerable<Bid>>();
            }
            else
            {
                // Handle the error response
                throw new Exception($"Failed to get item with ID {auctionId}. Status code: {response.StatusCode}");
            }
        }
    }
}


