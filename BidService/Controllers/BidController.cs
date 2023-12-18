using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BidService.Models;
using BidService.Services;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using RabbitMQ.Client;
using BidService;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Diagnostics;

namespace BidService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidController : ControllerBase
    {
        private readonly IBidRepository _BidRepository;
        private readonly ILogger<BidController> _logger;
        private readonly ICustomerRepository _customerRepository;
        private string _mqHost = string.Empty;

        public BidController(IBidRepository BidRepository, ICustomerRepository customerRepository, ILogger<BidController> logger)
        {
            _BidRepository = BidRepository;
            _customerRepository = customerRepository;
            _logger = logger;
            _mqHost = Environment.GetEnvironmentVariable("rabbitmq") ?? "localhost";
        }

        [HttpGet]
        public Task<ActionResult> GetTest()
        {
            return Task.FromResult<ActionResult>(Ok("BidService is running..."));
        }


        [HttpGet("{id}")]
        public Task<IActionResult> GetBidsForAuction(string id)
        {
            _logger.LogInformation($"### GetBidsForAuction: {id}");
            var Bids = _BidRepository.GetBidsForAuction(id).Result.ToList();
            _logger.LogInformation($"### GetBidsForAuction: {Bids.Count}");
            foreach (var Bid in Bids)
            {
                Bid.Customer = _customerRepository.GetCustomerById(Bid.Customer.Id).Result;
            }
            return Task.FromResult<IActionResult>(Ok(Bids));
        }

        [HttpPost]
        public ActionResult<Bid> PostBid(Bid bid)
        {
            _logger.LogInformation("posting..");
            try
            {
                _logger.LogInformation($"### PostBid: {_mqHost}");
                var factory = new ConnectionFactory();
                factory.Uri = new Uri(_mqHost);
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: "bids",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = JsonSerializer.Serialize(bid);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: string.Empty,
                                     routingKey: "bids",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine($" [x] Sent {message}");

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"exception: {ex.Message}");
                return StatusCode(500, $"{ex.Message}");
            }
            _logger.LogInformation($"OK: bid posted");
            return Ok(bid);
        }
    }
}