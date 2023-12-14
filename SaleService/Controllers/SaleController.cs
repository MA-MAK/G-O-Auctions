using Microsoft.AspNetCore.Mvc;
using SaleService.Models;
using SaleService.Services;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace SaleService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ILogger<SaleController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAuctionRepository _auctionRepository;

        public SaleController(
            ISaleRepository saleRepository,
            ILogger<SaleController> logger,
            IConfiguration configuration,
            IAuctionRepository auctionRepository
        )
        {
            _saleRepository = saleRepository;
            _logger = logger;
            _configuration = configuration;
            _auctionRepository = auctionRepository;
        }

        [HttpGet("{saleId}")]
        public async Task<IActionResult> GetSaleById(string saleId)
        {
            try
            {

                var sale = await _saleRepository.GetSaleById(saleId);
                _logger.LogInformation($"### Sale with ID {sale.Id} found.");

                if (sale == null)
                {
                    _logger.LogInformation($"Sale with ID {saleId} not found.");
                    return NotFound();
                }

                // Retrieve the associated auction
               // Auction auction = await _auctionRepository.GetAuctionById(sale.Auction.Id);
                //sale.Auction = auction;

                return Ok(sale);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting sale with ID {saleId}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> PostSale([FromBody] Sale sale)
        {
            try
            {
                // Check if the auction with the provided ID exists
                Auction auction = await _auctionRepository.GetAuctionById(sale.Auction.Id);

                if (auction == null)
                {
                    _logger.LogError($"### Auction with ID {sale.Auction.Id} not found.");
                    throw new Exception($"Auction with ID {sale.Auction.Id} not found.");
                }
                // Add logic to validate the sale object if needed

                await _saleRepository.PostSale(sale);

                // Return the created sale
                return CreatedAtAction(nameof(GetSaleById), new { saleId = sale.Id }, sale);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating sale: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
