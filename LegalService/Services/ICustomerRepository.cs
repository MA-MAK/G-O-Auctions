using LegalService.Models;

namespace LegalService.Services
{
    public interface ICustomerRepository
    {
        public Task<Customer> GetCustomerById(string customerId);
    }
}
