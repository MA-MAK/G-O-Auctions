using CustomerService.Models;

namespace CustomerService.Services
{
    public interface ICustomerRepository
    {
        public Task<Customer> GetCustomerById(string customerId);
        public Task<bool> PostCustomer(Customer Customer);
    }
}
