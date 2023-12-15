using System.Collections.Generic;
using LegalService.Models;
using MongoDB.Driver;
using System.Text.Json;
using System.Text;


namespace LegalService.Services
{
    public class AuthRepository : IAuthRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthRepository> _logger;

        public AuthRepository(HttpClient httpClient, ILogger<AuthRepository> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<HttpResponseMessage> Login(LoginModel login)
        {
            try
            {
                // Make a GET request to the API endpoint with the item ID
                HttpResponseMessage response = await _httpClient.PostAsync($"/api/legal/login", new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    //string jsonString = await response.Content.ReadAsStringAsync();
                    //Customer customer = JsonSerializer.Deserialize<Customer>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return response;
                }
                else
                {
                    _logger.LogError(
                        $"### Status code: {response.StatusCode}"
                    );
                    // Handle the error response
                    throw new Exception(
                        $"Status code: {response.StatusCode}"
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    $"### {ex.Message}"
                );
                // Log and handle the exception appropriately
                throw new Exception($"{ex.Message}", ex);
            }
        }
    }
}
