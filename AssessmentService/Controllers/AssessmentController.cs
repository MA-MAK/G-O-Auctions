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
        public async Task<IActionResult> GetItemById(int itemId)
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
                var items = await _assessmentRepository.GetAllRegistredItems(Status.Registered);
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("PostItem")]
        public async Task<IActionResult> PostItem(Item item)
        {
            try
            {
                // Assuming there's a PUT function in ItemService to update items
                var result = await _assessmentRepository.PostItem(item);

                if (result)
                {
                    return Ok($"Item with ID {item.Id} posted successfully");
                }
                else
                {
                    return BadRequest($"Failed to post item with ID {item.Id}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
