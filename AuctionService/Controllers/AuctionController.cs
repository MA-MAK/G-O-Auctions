using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AuctionService.Models;
using AuctionService.Services;

namespace AuctionService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionController : ControllerBase
    {
        private readonly List<Auction> _auctions;
        private readonly ILogger<AuctionController> _logger;
        private readonly IAuctionRepository _auctionRepository;

        public AuctionController()
        {
            _auctions = new List<Auction>();
        }

        public AuctionController(ILogger<AuctionController> logger, IConfiguration configuration, IAuctionRepository auctionRepository)
    {
        _logger = logger;
        _auctionRepository = auctionRepository;
    }

        // GET: api/auction
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_auctions);
        }

        // GET: api/auction/{id}
        [HttpGet("{id}")]
        public IActionResult GetAuctionById(int id)
        {
            var auction = _auctions.Find(a => a.Id == id);
            if (auction == null)
            {
                return NotFound();
            }
            return Ok(auction);
        }

        // POST: api/auction
        [HttpPost]
        public IActionResult Post([FromBody] Auction auction)
        {
            _auctions.Add(auction);
            return CreatedAtAction(nameof(Get), new { id = auction.Id }, auction);
        }

        // PUT: api/auction/{id}
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Auction auction)
        {
            var existingAuction = _auctions.Find(a => a.Id == id);
            if (existingAuction == null)
            {
                return NotFound();
            }
            //existingAuction.Name = auction.Name;
            //existingAuction.Description = auction.Description;
            return NoContent();
        }

        // DELETE: api/auction/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var auction = _auctions.Find(a => a.Id == id);
            if (auction == null)
            {
                return NotFound();
            }
            _auctions.Remove(auction);
            return NoContent();
        }
    }
}
