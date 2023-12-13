using BidService.Models;

namespace BidService.Services;
public interface ICustomerRepository
{
    Task<Customer> GetCustomerById(string CustomerId);
}
