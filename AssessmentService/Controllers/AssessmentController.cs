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
        
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<AssessmentController> _logger;
        private readonly IConfiguration _configuration;

        public AssessmentController(IAssessmentRepository assessmentRepository, ILogger<AssessmentController> logger, IConfiguration configuration)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetItemById(string itemId)
        {
            try
            {
                var item = await _assessmentRepository.GetItemById(itemId);

                if (item != null)
                {
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
                var items = await _assessmentRepository.GetAllRegistredItems();

                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("PutItem")]
        public async Task<IActionResult> UpdateItem(string itemId, string description, int year, decimal assessmentPrice, int category, int condition, int status, string title)
        {
            try
            {
                // Assuming there's a PUT function in ItemService to update items
                await _assessmentRepository.UpdateItem(itemId, description, year, assessmentPrice, category, condition, status, title);

                // If the update is successful, return Ok
                return Ok($"Item with ID {itemId} updated successfully");
            }
            catch (KeyNotFoundException)
            {
                // Handle the case where the item with the specified ID is not found
                return NotFound($"Item with ID {itemId} not found");
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
