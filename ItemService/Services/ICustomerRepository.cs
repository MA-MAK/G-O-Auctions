using ItemService.Models;

namespace ItemService.Services;
public interface ICustomerRepository
{
    Task<Customer> GetCustomerById(string CustomerId);
}
