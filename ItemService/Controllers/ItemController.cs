using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ItemService.Models;
using ItemService.Services;

namespace ItemService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;
        private readonly ILogger<ItemController> _logger;
        private readonly IConfiguration _configuration;

        public ItemController(IItemRepository itemRepository, ILogger<ItemController> logger, IConfiguration configuration)
        {
            _itemRepository = itemRepository;
            _logger = logger;
            _configuration = configuration;
        }
        
        [HttpGet("{id}")]
        public Task<IActionResult> GetItemById(string id)
        {
            _logger.LogInformation($"ItemController.GetItemById - id: {id}");
            var item = _itemRepository.GetItemById(id).Result;
            _logger.LogInformation($"ItemController.GetItemById - item: {item}");
            return Task.FromResult<IActionResult>(Ok(item));
        }
    }
}
