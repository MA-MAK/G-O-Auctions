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

        public BidController(IBidRepository bidRepository)
        {
            _bidRepository = bidRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Bid>> GetAllBids()
        {
            var bids = _bidRepository.GetAllBids();
            return Ok(bids);
        }

        [HttpGet("{id}")]
        public ActionResult<Bid> GetBidById(int id)
        {
            var bid = _bidRepository.GetBidById(id);
            if (bid == null)
            {
                return NotFound();
            }
            return Ok(bid);
        }

        [HttpPost]
        public ActionResult<Bid> CreateBid(Bid bid)
        {
            _bidRepository.CreateBid(bid);
            return CreatedAtAction(nameof(GetBidById), new { id = bid.Id }, bid);
        }

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
    }
}
