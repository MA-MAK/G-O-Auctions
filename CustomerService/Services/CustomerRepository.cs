using System.Collections.Generic;
using CustomerService.Models;
using MongoDB.Driver;

namespace CustomerService.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IMongoCollection<Customer> _customers;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(MongoDBContext dbContext, ILogger<CustomerRepository> logger)
        {
            _customers = dbContext.Customers;
            _logger = logger;
        }

        public Task<Customer> GetCustomerById(string id)
        {
            _logger.LogInformation($"### CustomerRepository.GetCustomerById - customerId: {id}");
            Customer customer = _customers.Find(c => c.Id == id).FirstOrDefault();
            _logger.LogInformation($"### CustomerRepository.GetCustomerById - customerId: {customer.Id}");
            return Task.FromResult<Customer>(customer);
        }
    }
}