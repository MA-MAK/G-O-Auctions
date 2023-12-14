using NLog;
using NLog.Web;
using SaleService.Services;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using SaleService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("AuctionService", client =>
    {
        client.BaseAddress = new Uri("http://localhost:5064");
        // Add any additional configuration for HttpClient as needed
    });

builder.Services.AddSingleton<IAuctionRepository, AuctionRepository>(
     b => new AuctionRepository(b.GetService<IHttpClientFactory>()
       .CreateClient("AuctionService"),
       builder.Services.BuildServiceProvider().GetRequiredService<ILogger<AuctionRepository>>()));
var connectionString = builder.Configuration.GetConnectionString("MongoDBConnection");
var databaseName = builder.Configuration.GetSection("MongoDBSettings:DatabaseName").Value;
var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<MongoDBContext>>();
builder.Services.AddSingleton<MongoDBContext>(provider => new MongoDBContext(logger, builder.Configuration));
builder.Services.AddSingleton<ISaleRepository, SaleRepository>();
/*builder.Services.AddSingleton<ISaleRepository, SaleRepository>(
    a => new SaleRepository(builder.Services.BuildServiceProvider().GetRequiredService<MongoDBContext>(), 
    builder.Services.BuildServiceProvider().GetRequiredService<ILogger<SaleRepository>>(),
    builder.Services.BuildServiceProvider().GetRequiredService<IAuctionRepository>()));*/
var app = builder.Build();

// public SaleRepository(MongoDBContext dbContext, ILogger<SaleRepository> logger, IAuctionRepository auctionRepository)
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
