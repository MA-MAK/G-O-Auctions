using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<IActionResult> GetItemById(string id)
        {
            _logger.LogInformation($"ItemController.GetItemById - id: {id}");
            var item = await _itemRepository.GetItemById(id);
            _logger.LogInformation($"ItemController.GetItemById - item: {item}");

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllItems()
        {
            try
            {
                var allItems = await _itemRepository.GetAllItems();
                return Ok(allItems);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while getting all items: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

         [HttpPut]
        public async Task<IActionResult> UpdateItem(Item updatedItem)
        {
            try
            {
                if (updatedItem == null)
                {
                    return BadRequest("Invalid item data");
                }

                /* var existingItem = await _itemRepository.GetItemById(id);

                if (existingItem == null)
                {
                    return NotFound();
                }
*/
                var success = await _itemRepository.UpdateItem(updatedItem);
                _logger.LogInformation($"### ItemController: updateItem - response: {success}");
                if (success)
                {
                    return Ok(updatedItem);
                }

                return StatusCode(500, "Failed to update item");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while updating item: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}

