using System.Collections.Generic;
using SaleService.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Formatting;


namespace SaleService.Services;

public class CustomerRepository : ICustomerRepository
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CustomerRepository> _logger;

    public CustomerRepository(HttpClient httpClient, ILogger<CustomerRepository> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _logger.LogInformation($"### CustomerRepository - _httpClient: {_httpClient.BaseAddress}");

    }

    public async Task<Customer> GetCustomerById(string customerId)
    {
        _logger.LogInformation($"### CustomerRepository.GetCustomerById - id: {customerId}");

        try
        {
            // Make a GET request to the API endpoint with the AuctionID
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/customer/{customerId}");
            _logger.LogInformation($"### CustomerRepository.GetCustomerById - response: {response}");
            if (response.IsSuccessStatusCode)
            {
                // _logger.LogInformation($"### AuctionRepository.GetAuctionById - response: {response}");

                string jsonString = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"### CustomerRepository.GetCustomerById - jsonString: {jsonString}");

                Customer customer = JsonSerializer.Deserialize<Customer>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                _logger.LogInformation($"### CustomerRepository.GetCustomerById - Customer: {customer.Id}");
                return customer;
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"### Failed to get customer with ID {customerId}. Status code: {response.StatusCode}, Content: {errorContent}");
                // Handle the error response
                throw new Exception($"Failed to get customer with ID {customerId}. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"### Error in GetCustomerById for customer with ID {customerId}: {ex.Message}");
            // Log and handle the exception appropriately
            throw new Exception($"Error in GetCustomerById: {ex.Message}", ex);
        }
    }
}