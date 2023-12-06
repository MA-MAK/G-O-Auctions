using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using AuctionService.Models;
using NLog.Fluent;
using System.Text.Json;

namespace AuctionService.Services
{
    public class BidRepository : IBidRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BidRepository> _logger;

        public BidRepository(HttpClient httpClient, ILogger<BidRepository> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            _logger.LogInformation($"### BidRepository - _httpClient: {_httpClient.BaseAddress}");
        }

        public async Task<IEnumerable<Bid>> GetBidsForAuction(string auctionId)
        {
            _logger.LogInformation($"### BidRepository.GetBidsForAuction - auctionId: {auctionId}");
            try
            {
                // Make a GET request to the API endpoint with the item ID
                HttpResponseMessage response = await _httpClient.GetAsync($"/api/bid/{auctionId}");
                _logger.LogInformation(
                    $"### BidRepository.GetBidsForAuction - response: {response}"
                );
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response content to an Item object
                    //Item item = await response.Content.ReadAsAsync<Item>();

                    string jsonString = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation(
                        $"### BidRepository.GetBidsForAuction - jsonString: {jsonString}"
                    );
                    //Item item = JsonSerializer.Deserialize<Item>(jsonString);
                    List<Bid> bids = JsonSerializer
                        .Deserialize<IEnumerable<Bid>>(
                            jsonString,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                        )
                        .ToList();
                    //_logger.LogInformation($"### BidRepository.GetBidsForAuction - bid: {bid.Id}");
                    return bids;
                }
                else
                {
                    _logger.LogError(
                        $"### Failed to get bids with auction ID {auctionId}. Status code: {response.StatusCode}"
                    );
                    // Handle the error response
                    throw new Exception(
                        $"Failed to get bids with auction ID {auctionId}. Status code: {response.StatusCode}"
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    $"### Error in GetBidsForAuction for auction with ID {auctionId}: {ex.Message}"
                );
                // Log and handle the exception appropriately
                throw new Exception($"Error in GetBidsForAuction: {ex.Message}", ex);
            }
        }
    }
}
