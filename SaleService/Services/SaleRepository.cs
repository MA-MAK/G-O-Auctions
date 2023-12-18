using System.Collections.Generic;
using SaleService.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Formatting;


namespace SaleService.Services
{
    public class SaleRepository : ISaleRepository
    {
        private readonly IMongoCollection<Sale> _sales;
        private readonly ILogger<SaleRepository> _logger;
        private readonly IAuctionRepository _auctionRepository;

        public SaleRepository(MongoDBContext dbContext, ILogger<SaleRepository> logger, IAuctionRepository auctionRepository)
        {
            _sales = dbContext.Sales;
            _logger = logger;
            _auctionRepository = auctionRepository;
        }

        public Task<Sale> GetSaleById(string saleId)
        {
            _logger.LogInformation($"### SaleRepository.GetSaleById - saleId: {saleId}");
            // Make a GET request to the API endpoint with the SaleID
            Sale sale = _sales.Find<Sale>(sale => sale.Id == saleId).FirstOrDefault();
            _logger.LogInformation($"### SaleRepository.GetSaleById - sale > auction id: {sale.Auction.Id}");
            _logger.LogInformation($"### SaleRepository.GetSaleById - sale > customer id: {sale.Customer.Id}");
            return Task.FromResult<Sale>(sale);
        }

        public async Task PostSale(Sale sale)
        {
            _logger.LogInformation($"### SaleRepository.PostSale");

            try
            {
                // Add the sale to the sales collection
                await _sales.InsertOneAsync(sale);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"### Error in PostSale: {ex.Message}");
                // Log and handle the exception appropriately
                throw new Exception($"Error in PostSale: {ex.Message}", ex);
            }
        }
    }
}