using BidWorker.Models;

namespace BidWorker.Services;
public interface ICustomerRepository
{
    Task<Customer> GetCustomerById(string CustomerId);
}
