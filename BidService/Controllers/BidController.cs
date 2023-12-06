using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BidService.Models;
using BidService.Services;

namespace BidService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidController : ControllerBase
    {
        private readonly IBidRepository _bidRepository;
        private readonly ILogger<BidController> _logger;
        private readonly IConfiguration _configuration;

        public BidController(IBidRepository bidRepository, ILogger<BidController> logger, IConfiguration configuration)
        {
            _bidRepository = bidRepository;
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
            return Task.FromResult<IActionResult>(Ok(bids));

        }

/*
        [HttpPost]
        public ActionResult<Bid> CreateBid(Bid bid)
        {
            _bidRepository.CreateBid(bid);
            return CreatedAtAction(nameof(GetBidById), new { id = bid.Id }, bid);
        }
*/
/*
        [HttpPut("{id}")]
        public IActionResult UpdateBid(int id, Bid bid)
        {
            if (id != bid.Id)
            {
                return BadRequest();
            }
            _bidRepository.UpdateBid(bid);
            return NoContent();
        }
*/
/*
        [HttpDelete("{id}")]
        public IActionResult DeleteBid(int id)
        {
            var bid = _bidRepository.GetBidById(id);
            if (bid == null)
            {
                return NotFound();
            }
            _bidRepository.DeleteBid(bid);
            return NoContent();
        }
        */
    }
}
