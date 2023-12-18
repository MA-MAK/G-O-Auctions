using BidWorker.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BidWorker.Models;
using BidWorker.Services;
using BidWorker;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Microsoft.Extensions.Logging;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddLogging();
        services.AddSingleton<MongoDBContext>(provider => new MongoDBContext(provider.GetService<ILogger<MongoDBContext>>()));
        services.AddSingleton<IBidRepository, BidRepository>(provider => new BidRepository(provider.GetService<MongoDBContext>(), provider.GetService<ILogger<BidRepository>>()));
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
