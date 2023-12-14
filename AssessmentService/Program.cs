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
builder.Services.AddHttpClient("AssessmentService", client =>
    {
        client.BaseAddress = new Uri("http://localhost:5164");
        // Add any additional configuration for HttpClient as needed
    });
builder.Services.AddSingleton<IAssessmentRepository, AssessmentRepository>(
       b => new AssessmentRepository(b.GetService<IHttpClientFactory>()
       .CreateClient("AssessmentService"),
       builder.Services.BuildServiceProvider().GetRequiredService<ILogger<AssessmentRepository>>()));
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