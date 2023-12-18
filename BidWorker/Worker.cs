using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using MongoDB.Bson;
using MongoDB.Driver;
using BidWorker.Services;
using BidWorker.Models; 

namespace BidWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private string _mqHost = string.Empty;
    private readonly BidRepository _bidRepository;
    public Worker(ILogger<Worker> logger, IBidRepository bidRepository)
    {
        _logger = logger;
        _mqHost = configuration["rabbitmqHost"] ?? "localhost";
        _logger.LogInformation($"Connecting to host: {_mqHost}");
        _bidRepository = bidRepository ?? throw new ArgumentNullException(nameof(bidRepository));

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Connecting to host: {_mqHost}", DateTimeOffset.Now);
        var factory = new ConnectionFactory { HostName = _mqHost }; // indsæt miljø varibel // noget alle 
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        // Declare the "booking" queue
        channel.QueueDeclare(queue: "bids",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);


        // Set up consumer for the "booking" queue
        var bidConsumer = new EventingBasicConsumer(channel);
         _logger.LogInformation($"listening to: {channel.ToString()}");
        bidConsumer.Received += (model, ea) =>
        {

            var body = ea.Body.ToArray();
             _logger.LogInformation($"received body: {body}");
            var message = Encoding.UTF8.GetString(body);
             _logger.LogInformation($"received message: {message}");
            var bid = System.Text.Json.JsonSerializer.Deserialize<Bid>(message);
            _logger.LogInformation($"Received bid: {bid}");
            _bidRepository.InsertBid(bid);
        };

        // Start consuming messages from the "booking" queue
        channel.BasicConsume(queue: "bids",
                             autoAck: true,
                             consumer: bidConsumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            // _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
