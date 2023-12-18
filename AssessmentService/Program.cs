using NLog;
using NLog.Web;
using AssessmentService.Services;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using AssessmentService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("ItemService", client =>
    {
        client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("ItemService") ?? "http://localhost:5004");
    });
builder.Services.AddSingleton<IItemRepository, ItemRepository>(
       b => new ItemRepository(b.GetService<IHttpClientFactory>()
       .CreateClient("ItemService"),
       builder.Services.BuildServiceProvider().GetRequiredService<ILogger<ItemRepository>>()));

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