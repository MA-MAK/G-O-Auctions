using System.Threading.Tasks;
using SaleService.Models;

namespace SaleService.Services
{
    public interface ISaleRepository
    {
        Task<Sale> GetSaleById(string saleId);
        Task PostSale(Sale sale);
    }
}
