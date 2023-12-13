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

        public Task<Sale> GetSalById(string itemId)
        {
            var sale = _sales.Find(a => a.Id == id).FirstOrDefaultAsync();
            return Task.FromResult<Sale>(sale.Result);
        }
    }
}
