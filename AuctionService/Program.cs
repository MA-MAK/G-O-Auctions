using NLog;
using NLog.Web;
using AuctionService.Services;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using AuctionService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.Commons;

//var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
//logger.Debug("init main");
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    builder.Services.AddLogging();
    builder.Services.AddHttpClient(
        "ItemService",
        client =>
        {
            client.BaseAddress = new Uri("http://localhost:5164");
        }
    );

    builder.Services.AddHttpClient(
        "BidService",
        client =>
        {
            client.BaseAddress = new Uri("http://localhost:5223");
        }
    );

    builder.Services.AddSingleton<IAuctionRepository, AuctionRepository>();

    builder.Services.AddSingleton<IItemRepository, ItemRepository>(
        b =>
            new ItemRepository(
                b.GetService<IHttpClientFactory>().CreateClient("ItemService"),
                builder.Services
                    .BuildServiceProvider()
                    .GetRequiredService<ILogger<ItemRepository>>()
            )
    );

    builder.Services.AddSingleton<IBidRepository, BidRepository>(
        b =>
            new BidRepository(
                b.GetService<IHttpClientFactory>().CreateClient("BidService"),
                builder.Services.BuildServiceProvider().GetRequiredService<ILogger<BidRepository>>()
            )
    );

    // MongoDB configuration
    var connectionString = builder.Configuration.GetConnectionString("MongoDBConnection");
    var databaseName = builder.Configuration.GetSection("MongoDBSettings:DatabaseName").Value;
    var logger = builder.Services
        .BuildServiceProvider()
        .GetRequiredService<ILogger<MongoDBContext>>();
    builder.Services.AddSingleton<MongoDBContext>(
        provider => new MongoDBContext(logger, builder.Configuration)
    );

    // Vault
    var EndPoint = Environment.GetEnvironmentVariable("vault") ?? "https://localhost:8201/";
    var httpClientHandler = new HttpClientHandler();
    httpClientHandler.ServerCertificateCustomValidationCallback = (
        message,
        cert,
        chain,
        sslPolicyErrors
    ) =>
    {
        return true;
    };

    // Initialize one of the several auth methods.
    IAuthMethodInfo authMethod = new TokenAuthMethodInfo("00000000-0000-0000-0000-000000000000");
    // Initialize settings. You can also set proxies, custom delegates etc. here.
    var vaultClientSettings = new VaultClientSettings(EndPoint, authMethod)
    {
        Namespace = "",
        MyHttpClientProviderFunc = handler =>
            new HttpClient(httpClientHandler) { BaseAddress = new Uri(EndPoint) }
    };
    IVaultClient vaultClient = new VaultClient(vaultClientSettings);

    // Use client to read a key-value secret.
    Secret<SecretData> kv2Secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(
        path: "auctionSecrets",
        mountPoint: "secret"
    );
    string mySecret = kv2Secret.Data.Data["Secret"].ToString();
    // Console.WriteLine($"MinKode: {minkode}");
    string myIssuer = Environment.GetEnvironmentVariable("Issuer") ?? "http://localhost:5050";
    var _logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
    _logger.LogInformation($"### myIssuer {myIssuer}");
    _logger.LogInformation($"### mySecret {mySecret}");

    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = myIssuer,
                ValidAudience = "http://localhost",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(mySecret))
            };
        });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();
    app.UseAuthentication();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    //logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}
