using SaleService.Models;
public interface ICustomerRepository
{
    Task<Customer> GetCustomerById(string customerId);
}
