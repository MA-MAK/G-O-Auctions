using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        public async Task<IActionResult> GetCustomerById(string id)
        {
            _logger.LogInformation($"CustomerController.GetCustomerById - id: {id}");
            var customer = await _customerRepository.GetCustomerById(id);

            if (customer == null)
            {
                return NotFound();
            }

            _logger.LogInformation($"CustomerController.GetCustomerById - customer: {customer}");
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> PostCustomer(Customer newCustomer)
        {
            try
            {
                if (newCustomer == null)
                {
                    return BadRequest("Invalid customer data");
                }

                // Perform any additional validation if needed

                var success = await _customerRepository.PostCustomer(newCustomer);

                if (success)
                {
                    return CreatedAtAction(nameof(GetCustomerById), new { id = newCustomer.Id }, newCustomer);
                }

                return StatusCode(500, "Failed to post customer");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while posting customer: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
