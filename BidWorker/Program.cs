using BidWorker.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BidWorker.Models;
using BidWorker;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

//var connectionString = builder.Configuration.GetConnectionString("MongoDBConnection");
//var databaseName = builder.Configuration.GetSection("MongoDBSettings:DatabaseName").Value;
//var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<MongoDBContext>>();


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddLogging();
        IConfiguration configuration = host.Configuration;
        services.AddHostedService<Worker>();
        services.AddSingleton<BidRepository>();
        services.AddSingleton<MongoDBContext>(provider => new MongoDBContext(logger, host.Configuration));
    })
    .Build();

host.Run();
