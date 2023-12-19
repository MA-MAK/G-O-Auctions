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
        client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("AuctionService") ?? "http://localhost:5001");
        // Add any additional configuration for HttpClient as needed
    });
builder.Services.AddSingleton<IAuctionRepository, AuctionRepository>(
     b => new AuctionRepository(b.GetService<IHttpClientFactory>()
       .CreateClient("AuctionService"),
       builder.Services.BuildServiceProvider().GetRequiredService<ILogger<AuctionRepository>>()));

builder.Services.AddHttpClient("CustomerService", client =>
    {
        client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("CustomerService") ?? "http://localhost:5003");
        // Add any additional configuration for HttpClient as needed
    });
builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>(
     b => new CustomerRepository(b.GetService<IHttpClientFactory>()
       .CreateClient("CustomerService"),
       builder.Services.BuildServiceProvider().GetRequiredService<ILogger<CustomerRepository>>()));

var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<MongoDBContext>>();
builder.Services.AddSingleton<MongoDBContext>(provider => new MongoDBContext(logger));
builder.Services.AddSingleton<ISaleRepository, SaleRepository>();

var app = builder.Build();

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
