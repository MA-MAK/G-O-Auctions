using BidService.Models;

public interface ICustomerRepository
{
    Task<Customer> GetCustomerForBid(int BidId);
}
