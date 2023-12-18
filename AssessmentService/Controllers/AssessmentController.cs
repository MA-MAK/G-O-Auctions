using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssessmentService.Models;
using AssessmentService.Services;

namespace AssessmentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssessmentController : ControllerBase
    {
        private readonly ILogger<AssessmentController> _logger;
        private readonly IItemRepository _itemRepository;

        public AssessmentController(ILogger<AssessmentController> logger, IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
            _logger = logger;
        }

        [HttpGet]
        public Task<ActionResult> GetTest()
        {
            return Task.FromResult<ActionResult>(Ok("ItemService is running..."));
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetItemById(string itemId)
        {
            try
            {
                _logger.LogInformation($"### AssessmentController.GetItemById - itemId: {itemId}");
                var item = await _itemRepository.GetItemById(itemId);

                if (item != null)
                {
                    _logger.LogInformation($"### AssessmentController.GetItemById - item: {item.Id}");
                    return Ok(item);
                }
                else
                {
                    return NotFound($"Item with ID {itemId} not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllRegistredItems")]
        public async Task<IActionResult> GetAllRegistredItems()
        {
            try
            {
                var items = await _itemRepository.GetAllRegistredItems();
                _logger.LogInformation($"### AssessmentController.GetAllRegistredItems - items: {items.Count()}");

                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateItem(Item updatedItem)
        {
            _logger.LogInformation($"### AssessmentController.UpdateItem - item: {updatedItem.Id}");

            try
            {
                // Assuming there's a PUT function in ItemService to update items
                await _itemRepository.UpdateItem(updatedItem);
                _logger.LogInformation($"### AssessmentController.UpdateItem - item: {updatedItem.Id}");

                // If the update is successful, return Ok
                return Ok(updatedItem);
            }
            catch (KeyNotFoundException)
            {
                // Handle the case where the item with the specified ID is not found
                return NotFound($"Item with ID {updatedItem.Id} not found");
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                _logger.LogError($"Exception: {ex.Message}");

                // Return a generic error message for other exceptions
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
