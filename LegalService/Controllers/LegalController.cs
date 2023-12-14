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

        private readonly IConfiguration _configuration;

        private readonly ICustomerRepository _customerRepository;

        public LegalController(
            IAuctionRepository auctionRepository,
            ICustomerRepository customerRepository,
            ILogger<LegalController> logger,
            IConfiguration configuration
        )
        {
            _auctionRepository = auctionRepository;
            _customerRepository = customerRepository;
            _logger = logger;
            _configuration = configuration;
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
/*
        // GET: api/legal/user/{id}
        [HttpGet("{id}")]
        public Task<IActionResult> GetCustomerById(string id)
        {
            _logger.LogInformation($"CustomerController.GetCustomerById - id: {id}");
            var customer = _customerRepository.GetCustomerById(id).Result;
            _logger.LogInformation($"CustomerController.GetCustomerById - customer: {customer}");
            return Task.FromResult<IActionResult>(Ok(customer));
        }
        */
    }
}
