using System.Collections.Generic;
using LegalService.Models;
using MongoDB.Driver;
using System.Text.Json;

namespace LegalService.Services
{
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
            _logger.LogInformation($"### CustomerRepository.GetCustomerById - customerId: {customerId}");

            try
            {
            // Make a GET request to the API endpoint with the item ID
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/customer/{customerId}");

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"### CustomerRepository.GetCustomerById - response: {response}");

                string jsonString = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"### CustomerRepository.GetCustomerById - jsonString: {jsonString}");
                Customer customer = JsonSerializer.Deserialize<Customer>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                _logger.LogInformation($"### CustomerRepository.GetCustomerById - customer: {customer.Id}");
                return customer;
            }
            else
            {
                _logger.LogError($"### Failed to get customer with ID {customerId}. Status code: {response.StatusCode}");
                // Handle the error response
                throw new Exception($"Failed to get item customer ID {customerId}. Status code: {response.StatusCode}");
            }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"### Error in GetCustomerById for item ID {customerId}: {ex.Message}");
                // Log and handle the exception appropriately
                throw new Exception($"Error in GetCustomerById: {ex.Message}", ex);
            }
        }
    }
}