using ItemService.Services;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using ItemService.Models;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddSingleton<IItemRepository, ItemRepository>();

    builder.Services.AddHttpClient("CustomerService", client =>
    {
        client.BaseAddress = new Uri("http://localhost:5104");
    });

    builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>(
        b => new CustomerRepository(b.GetService<IHttpClientFactory>()
        .CreateClient("CustomerService"), 
        builder.Services.BuildServiceProvider().GetRequiredService<ILogger<CustomerRepository>>()));

// MongoDB configuration
var connectionString = builder.Configuration.GetConnectionString("MongoDBConnection");
var databaseName = builder.Configuration.GetSection("MongoDBSettings:DatabaseName").Value;
var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<MongoDBContext>>();
builder.Services.AddSingleton<MongoDBContext>(provider => new MongoDBContext(logger, builder.Configuration));

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
