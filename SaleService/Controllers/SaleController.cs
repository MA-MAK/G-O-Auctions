using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SaleService.Models;
using SaleService.Services;

namespace SaleService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly ISaleRepository _saleRepository;
        
        private readonly ILogger<SaleController> _logger;
        private readonly IConfiguration _configuration;

        public SaleController(ISaleRepository saleRepository, ILogger<SaleController> logger, IConfiguration configuration)
        {
            _saleRepository = saleRepository;
            _logger = logger;
            _configuration = configuration;
        }
        
        [HttpGet("{id}")]
        public Task<IActionResult> GetSaleForItem(string itemId)
        {
            var sale = _saleRepository.GetSaleForItem(itemId).Result;
            return Task.FromResult<IActionResult>(Ok(sale));
        }
        /*
        [HttpPost]
        public async Task<IActionResult> PostSale(Sale sale)
        {
            // TODO: Implement the logic to create a new sale
            // and return the created sale object or appropriate response.
            // Example:
            // var createdSale = await _saleRepository.CreateSale(sale);
            // return CreatedAtAction(nameof(GetSaleForAuction), new { id = createdSale.Id }, createdSale);
            
            return Ok();
        }
        */
    }
}

        


