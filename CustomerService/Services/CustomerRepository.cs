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

        public async Task<bool> PostCustomer(Customer newCustomer)
        {
            try
            {
                // Check if a customer with the same ID already exists
                var existingCustomer = await GetCustomerById(newCustomer.Id);

                if (existingCustomer != null)
                {
                    _logger.LogWarning($"Customer with ID {newCustomer.Id} already exists.");
                    return false; // Customer already exists, post failed
                }

                // Insert the new customer
                await _customers.InsertOneAsync(newCustomer);

                return true; // Customer posted successfully
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while posting customer: {ex.Message}");
                return false;
            }

        }
    }
}