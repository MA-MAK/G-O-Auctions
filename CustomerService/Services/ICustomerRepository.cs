using CustomerService.Models;

namespace CustomerService.Services
{
    public interface ICustomerRepository
    {
        public Task<Customer> GetCustomerById(string customerId);
        Task<bool> PostCustomer(Customer newCustomer);
    }
}
