using SaleService.Models;

namespace SaleService.Services
{
    public interface ISaleRepository
    {
        public Task<Sale> GetSaleForItem(string itemId);
    }
}
