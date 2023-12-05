using SaleService.Models;

namespace SaleService.Services
{
    public interface ISaleRepository
    {
        public Task<Sale> GetSaleForAuction(int auctionId);
    }
}
