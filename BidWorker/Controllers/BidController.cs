using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BidWorker.Models;
using BidWorker.Services;


namespace BidWorker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidController : ControllerBase
    {
        private readonly IBidRepository _bidRepository;
        private readonly ILogger<BidController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ICustomerRepository _customerRepository;

        public BidController(IBidRepository bidRepository, ICustomerRepository customerRepository, ILogger<BidController> logger, IConfiguration configuration)
        {
            _bidRepository = bidRepository;
            _customerRepository = customerRepository;
            _logger = logger;
            _configuration = configuration;
        }
        /*
                [HttpGet]
                public ActionResult<IEnumerable<Bid>> GetAllBids()
                {
                    var bids = _bidRepository.GetAllBids();
                    return Ok(bids);
                }
        */
        [HttpGet("{id}")]
        public Task<IActionResult> GetBidsForAuction(string id)
        {
            _logger.LogInformation($"### GetBidsForAuction: {id}");
            var bids = _bidRepository.GetBidsForAuction(id).Result.ToList();
            _logger.LogInformation($"### GetBidsForAuction: {bids.Count}");
            foreach (var bid in bids)
            {
                bid.Customer = _customerRepository.GetCustomerById(bid.Customer.Id).Result;
            }
            return Task.FromResult<IActionResult>(Ok(bids));

        }

        [HttpPost]
        public async Task<IActionResult> PostBid(Bid newBid)
        {
            try
            {
                _logger.LogInformation($"### BidController.PostBid");

                // Validate the new bid and check for existing bids
                if (newBid == null || newBid.Amount <= 0 || string.IsNullOrEmpty(newBid.CustomerId) || string.IsNullOrEmpty(newBid.AuctionId))
                {
                    return BadRequest("Invalid bid data");
                }

                var success = await _bidRepository.PostBid(newBid);

                if (success)
                {
                    // Bid posted successfully
                    return CreatedAtAction(nameof(GetBidsForAuction), new { id = newBid.AuctionId }, newBid);
                }

                // Bid post failed
                return StatusCode(500, "Failed to post bid");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while posting bid: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}