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

        private readonly IAuthRepository _authRepository;

        private readonly ILogger<LegalController> _logger;

        private readonly IConfiguration _configuration;

        private readonly ICustomerRepository _customerRepository;

        public LegalController(
            IAuctionRepository auctionRepository,
            ICustomerRepository customerRepository,
            ILogger<LegalController> logger,
            IConfiguration configuration,
            IAuthRepository authRepository
        )
        {
            _auctionRepository = auctionRepository;
            _customerRepository = customerRepository;
            _logger = logger;
            _configuration = configuration;
            _authRepository = authRepository;
        }

        // GET: api/auction/{auctionId}
        [HttpGet("{auctionId}")]
        [ActionName("auctions")]
        public async Task<IActionResult> GetAuctionById(string auctionId)
        {
            _logger.LogInformation($"### AuctionController.GetAuctionById - auctionId: {auctionId}");
            try
            {
                var auction = await _auctionRepository.GetAuctionById(auctionId);

                if (auction != null)
                {
                    _logger.LogInformation(
                        $"### AuctionController.GetAuctionById - auction: {auction.Id}"
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
            _logger.LogInformation($"### AuctionController.GetAllAuctions");
            var auctions = _auctionRepository.GetAllAuctions().Result;
            /*
            foreach (var auction in auctions)
            {
                auction.Item = _itemRepository.GetItemById(auction.Item.Id).Result;
                auction.Bids = _bidRepository.GetBidsForAuction(auction.Id).Result.ToList();
            }
            */
            return Ok(auctions);
            
        }
        // GET: api/user/{customerId}
        [HttpGet("{customerId}")]
        [ActionName("users")]
        public async Task<IActionResult> GetCustomerById(string customerId)
        {
            _logger.LogInformation($"### CustomerController.GetCustomerById - customerId: {customerId}");
            try
            {
                var customer = await _customerRepository.GetCustomerById(customerId);

                if (customer != null)
                {
                    _logger.LogInformation(
                        $"### CustomerController.GetCustomerById - customer: {customer.Id}"
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

        // POST: api/legal/login
        [HttpPost]
        [ActionName("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            try
            {
                var response = await _authRepository.Login(login);

                if (response.IsSuccessStatusCode)
                {
                    return Ok(response.Content);
                }
                else
                {
                    return Unauthorized();
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
