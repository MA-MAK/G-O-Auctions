using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LegalService.Models;
using LegalService.Services;

namespace LegalService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LegalController : ControllerBase
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly ILogger<LegalController> _logger;
        private readonly ICustomerRepository _customerRepository;

        public LegalController(
            IAuctionRepository auctionRepository,
            ICustomerRepository customerRepository,
            ILogger<LegalController> logger
        )
        {
            _auctionRepository = auctionRepository;
            _customerRepository = customerRepository;
            _logger = logger;
        }

        [HttpGet]
        public Task<ActionResult> GetTest()
        {
            return Task.FromResult<ActionResult>(Ok("LegalService is running..."));
        }

        // GET: api/auction/{auctionId}
        [HttpGet("{auctionId}")]
        [ActionName("auction")]
        public async Task<IActionResult> GetAuctionById(string auctionId)
        {
            _logger.LogInformation($"### LegalController.GetAuctionById - auctionId: {auctionId}");
            try
            {
                var auction = await _auctionRepository.GetAuctionById(auctionId);

                if (auction != null)
                {
                    _logger.LogInformation(
                        $"### LegalController.GetAuctionById - auction: {auction.Id}"
                    );
                    return Ok(auction);
                }
                else
                {
                    return NotFound($"Auction with ID {auctionId} not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/auction/all
        [HttpGet]
        [ActionName("auctions")]
        public IActionResult GetAllAuctions()
        {
            _logger.LogInformation($"### LegalController.GetAllAuctions");
            var auctions = _auctionRepository.GetAllAuctions().Result;
            
            return Ok(auctions);
        }
        // GET: api/user/{customerId}
        [HttpGet("{customerId}")]
        [ActionName("user")]
        public async Task<IActionResult> GetCustomerById(string customerId)
        {
            _logger.LogInformation($"### LegalController.GetCustomerById - customerId: {customerId}");
            try
            {
                var customer = await _customerRepository.GetCustomerById(customerId);

                if (customer != null)
                {
                    _logger.LogInformation(
                        $"### LegalController.GetCustomerById - customer: {customer.Id}"
                    );
                    return Ok(customer);
                }
                else
                {
                    return NotFound($"Customer with ID {customerId} not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
