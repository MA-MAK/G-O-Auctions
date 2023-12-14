using System.Collections.Generic;
using LegalService.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.Json;

namespace LegalService.Services;

public class AuctionRepository : IAuctionRepository
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuctionRepository> _logger;

    public AuctionRepository(HttpClient httpClient, ILogger<AuctionRepository> logger)
    {
        _logger = logger;
        _httpClient = httpClient;
    }
 

    public async Task<Auction> GetAuctionById(string auctionId)
    {
        try
        {
            // Make a GET request to the API endpoint with the item ID
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/auction/{auctionId}");
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response content to an Item object
                //Item item = await response.Content.ReadAsAsync<Item>();

                string jsonString = await response.Content.ReadAsStringAsync();
                _logger.LogInformation(
                    $"### AuctionsRepository.GetAuctionById - jsonString: {jsonString}"
                );
                //Auction auction = JsonSerializer.Deserialize<Auction>(jsonString);
                Auction auction = JsonSerializer.Deserialize<Auction>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                _logger.LogInformation($"### AuctionsRepository.GetAuctionById - auction: {auction.Id}");
                return auction;
            }
            else
            {
                // Handle the error response
                throw new Exception($"Failed to get auctions. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            // Log and handle the exception appropriately
            throw new Exception($"Error in GetAllAuctions: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<Auction>> GetAllAuctions()
    {
        try
        {
            // Make a GET request to the API endpoint with the item ID
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/auction/all");
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response content to an Item object
                //Item item = await response.Content.ReadAsAsync<Item>();

                string jsonString = await response.Content.ReadAsStringAsync();
                _logger.LogInformation(
                    $"### BidRepository.GetBidsForAuction - jsonString: {jsonString}"
                );
                //Item item = JsonSerializer.Deserialize<Item>(jsonString);
                List<Auction> auctions = JsonSerializer
                    .Deserialize<IEnumerable<Auction>>(
                        jsonString,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    )
                    .ToList();
                //_logger.LogInformation($"### BidRepository.GetBidsForAuction - bid: {bid.Id}");
                return auctions;
            }
            else
            {
                // Handle the error response
                throw new Exception($"Failed to get auctions. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            // Log and handle the exception appropriately
            throw new Exception($"Error in GetAllAuctions: {ex.Message}", ex);
        }
    }
}
