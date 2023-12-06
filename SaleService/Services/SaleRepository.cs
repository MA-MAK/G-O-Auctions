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

        /*

        public Task<Sale> GetSaleForItem(string itemId)
        {
            return Task.FromResult<Sale>(sales.Where(b => b.ItemId == itemId).FirstOrDefault());
        }


        public async Task<Sale> GetSaleById(int saleId)
        {
            // Connect to MongoDB
            var client = new MongoClient("mongodb://localhost:27018");
            var database = client.GetDatabase("salesdb");
            var collection = database.GetCollection<Sale>("sales");

            // Create a filter to find the sale by ID
            var filter = Builders<Sale>.Filter.Eq(s => s.Id, saleId);

            // Find the sale in the collection
            var sale = await collection.Find(filter).FirstOrDefaultAsync();

            return sale;
        }
        */
    }
}