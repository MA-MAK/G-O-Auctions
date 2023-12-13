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

        public Task<Customer> GetCustomerById(string customerId)
        {
            return Task.FromResult<Customer>(_customers.Find(a => a.Id == customerId).FirstOrDefault());
        }
    }
}