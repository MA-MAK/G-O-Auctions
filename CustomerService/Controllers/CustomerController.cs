using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CustomerService.Models;
using CustomerService.Services;

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
            _logger.LogInformation($"CustomerController.GetCustomerById - id: {id}");
            var customer = _customerRepository.GetCustomerById(id).Result;
            _logger.LogInformation($"CustomerController.GetCustomerById - customer: {customer}");
            return Task.FromResult<IActionResult>(Ok(customer));
        }
    }
}
