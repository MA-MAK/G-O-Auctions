using System.Collections.Generic;
using SaleService.Models;

namespace SaleService.Services
{
    public class SaleRepository : ISaleRepository
    {
        private List<Sale> sales;

        public SaleRepository()
        {
            sales = new List<Sale>();
        }

        public Task<Sale> GetSaleForItem(string itemId)
        {
            return Task.FromResult<Sale>(sales.Where(b => b.ItemId == itemId).FirstOrDefault());
        }

        

/*
        public async Task<Sale> CreateSale(Sale sale)
        {
            // Serialize the sale object to JSON
            var json = JsonConvert.SerializeObject(sale);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Make a POST request to the API endpoint to create a new sale
            HttpResponseMessage response = await _httpClient.PostAsync("/api/sales", content);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response content to a Sale object
                Sale createdSale = await response.Content.ReadAsAsync<Sale>();
                return createdSale;
            }
            else
            {
                // Handle the error response
                throw new Exception($"Failed to create sale. Status code: {response.StatusCode}");
            }
        }
        */

        /*

        public async Task<Sale> GetSaleById(int saleId)
        {
            // Make a GET request to the API endpoint with the item ID
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/sales/{saleId}");

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response content to an Item object
                Sale sale = await response.Content.ReadAsAsync<Item>();
                return sale;
            }
            else
            {
                // Handle the error response
                throw new Exception($"Failed to get sale with ID {saleId}. Status code: {response.StatusCode}");
            }
        }
        */

        

    }
}