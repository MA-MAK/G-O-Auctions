using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CustomerService.Models;
using CustomerService.Services;
using System.Diagnostics;

namespace CustomerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerController> _logger;
        private readonly IConfiguration _configuration;

        public CustomerController(ICustomerRepository customerRepository, ILogger<CustomerController> logger, IConfiguration configuration)
        {
            _customerRepository = customerRepository;
            _logger = logger;
            _configuration = configuration;
        }
        
        [HttpGet("{id}")]
        public Task<IActionResult> GetCustomerById(string id)
        {
            _logger.LogInformation($"### CustomerController.GetCustomerById - id: {id}");
            Customer customer = _customerRepository.GetCustomerById(id).Result;
            _logger.LogInformation($"### CustomerController.GetCustomerById - customer: {customer.Id}");
            return Task.FromResult<IActionResult>(Ok(customer));
        }

        [HttpPost]
        public async Task<IActionResult> PostCustomer(Customer customer)
        {
            try
            {
                var success = await _customerRepository.PostCustomer(customer);

                if (success)
                {
                    return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
                }

                return StatusCode(500, "Failed to post item");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while posting item: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}