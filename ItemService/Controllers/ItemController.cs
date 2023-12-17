using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ItemService.Models;
using ItemService.Services;
using System.Diagnostics;

namespace ItemService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;
        private readonly ILogger<ItemController> _logger;
        private readonly IConfiguration _configuration;

        private readonly ICustomerRepository _customerRepository;

        public ItemController(
            IItemRepository itemRepository,
            ICustomerRepository customerRepository,
            ILogger<ItemController> logger,
            IConfiguration configuration
        )
        {
            _itemRepository = itemRepository;
            _logger = logger;
            _configuration = configuration;
            _customerRepository = customerRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(string id)
        {
            try
            {
                _logger.LogInformation($"ItemController.GetItemById - id: {id}");

                var item = await _itemRepository.GetItemById(id);

                // Check if the item is null
                if (item == null)
                {
                    return NotFound();
                }

                // Check if the Customer property is not null before accessing its Id
                if (item.Customer != null)
                {
                    _logger.LogInformation($"ItemController.GetItemById - item > customer: {item.Customer.Id}");
                    item.Customer = _customerRepository.GetCustomerById(item.Customer.Id).Result;
                }

                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while getting item by Id: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllItems()
        {
            try
            {
                var allItems = await _itemRepository.GetAllItems();

                foreach (var item in allItems)
                {
                    if (item.Customer != null)
                    {
                        item.Customer = _customerRepository.GetCustomerById(item.Customer.Id).Result;
                    }
                }

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

        [HttpPost]
        public async Task<IActionResult> PostItem(Item item)
        {
            _logger.LogInformation($"### ItemController.PostItem - Posting item...");
            try
            {
                if (item == null)
                {
                    return BadRequest("Invalid item data");
                }

                // Check if the Customer property is not null before accessing its Id
                if (item.Customer != null)
                {
                    _logger.LogInformation($"### ItemController.PostItem - Item.Customer: {item.Customer.Id}");
                    _logger.LogInformation($"### ItemController.PostItem - Item:  {item.Title}");
                    item.Customer = _customerRepository.GetCustomerById(item.Customer.Id).Result;
                    _logger.LogInformation($"### ItemController.PostItem - Customer: {item.Customer.Name}");
                }

                var success = await _itemRepository.PostItem(item);
                _logger.LogInformation($"### ItemController.PostItem - response: {success}");

                if (success)
                {
                    return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, item);
                }

                return StatusCode(500, "Failed to post item");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while posting item: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("version")]
        public async Task<Dictionary<string, string>> GetVersion()
        {
            _logger.LogInformation("posting..");
            var properties = new Dictionary<string, string>();
            var assembly = typeof(Program).Assembly;
            properties.Add("service", "GOAuctions");
            var ver =
                FileVersionInfo.GetVersionInfo(typeof(Program).Assembly.Location).ProductVersion
                ?? "N/A";
            properties.Add("version", ver);
            var hostName = System.Net.Dns.GetHostName();
            var ips = await System.Net.Dns.GetHostAddressesAsync(hostName);
            var ipa = ips.First().MapToIPv4().ToString() ?? "N/A";
            properties.Add("ip-address", ipa);
            return properties;
        }
    }
}
