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
            _logger.LogInformation($"### CustomerController.GetCustomerById - id: {id}");
            Customer customer = _customerRepository.GetCustomerById(id).Result;
            _logger.LogInformation($"### CustomerController.GetCustomerById - customer: {customer.Id}");
            return Task.FromResult<IActionResult>(Ok(customer));
        }

        [HttpGet("version")]
        public async Task<Dictionary<string, string>> GetVersion()
        {
            _logger.LogInformation("posting..");
            var properties = new Dictionary<string, string>();
            var assembly = typeof(Program).Assembly;
            properties.Add("service", "GOAuctions");
            var ver =
                FileVersionInfo.GetVersionInfo(typeof(Program).Assembly.Location).ProductVersion
                ?? "N/A";
            properties.Add("version", ver);
            var hostName = System.Net.Dns.GetHostName();
            var ips = await System.Net.Dns.GetHostAddressesAsync(hostName);
            var ipa = ips.First().MapToIPv4().ToString() ?? "N/A";
            properties.Add("ip-address", ipa);
            return properties;
        }
    }
}
