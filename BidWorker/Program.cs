using BidWorker.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BidWorker.Models;
using BidWorker;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<MongoDBContext>>();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<MongoDBContext>(provider => new MongoDBContext(logger));
    })
    .Build();

host.Run();
