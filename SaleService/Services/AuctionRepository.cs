using System.Collections.Generic;
using SaleService.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Formatting;


namespace SaleService.Services;

public class AuctionRepository : IAuctionRepository
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuctionRepository> _logger;

    public AuctionRepository(HttpClient httpClient, ILogger<AuctionRepository> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _logger.LogInformation($"### AuctionRepository - _httpClient: {_httpClient.BaseAddress}");

    }

    public async Task<Auction> GetAuctionById(string auctionId)
    {
        _logger.LogInformation($"### AuctionRepository.GetAuctionById - id: {auctionId}");

        try
        {
            // Make a GET request to the API endpoint with the AuctionID
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/auction/{auctionId}");
            _logger.LogInformation($"### AuctionRepository.GetAuctionById - response: {response}");
            if (response.IsSuccessStatusCode)
            {
                // _logger.LogInformation($"### AuctionRepository.GetAuctionById - response: {response}");

                string jsonString = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"### AuctionRepository.GetAuctionById - jsonString: {jsonString}");

                Auction auction = JsonSerializer.Deserialize<Auction>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                _logger.LogInformation($"### AuctionRepository.GetAuctionById - Auction: {auction.Id}");
                return auction;
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"### Failed to get auction with ID {auctionId}. Status code: {response.StatusCode}, Content: {errorContent}");
                // Handle the error response
                throw new Exception($"Failed to get auction with ID {auctionId}. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"### Error in GetAuctionById for auction with ID {auctionId}: {ex.Message}");
            // Log and handle the exception appropriately
            throw new Exception($"Error in GetAuctionById: {ex.Message}", ex);
        }
    }
}