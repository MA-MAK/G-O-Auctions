using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using BidService.Models;

namespace BidService.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly HttpClient _httpClient;

        public CustomerRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Customer> GetCustomerForBid(int bidId)
        {
            // Make a GET request to the API endpoint with the item ID
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/customer/{bidId}");

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response content to an Item object
                Customer customer = await response.Content.ReadAsAsync<Customer>();
                return Customer;
            }
            else
            {
                // Handle the error response
                throw new Exception($"Failed to get item with ID {bidId}. Status code: {response.StatusCode}");
            }
        }
    }
}


