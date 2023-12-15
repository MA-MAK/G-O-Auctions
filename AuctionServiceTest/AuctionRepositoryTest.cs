/*using NUnit.Framework;
using System;
using System.Threading.Tasks;
using AuctionService.Models;
using AuctionService.Services;
using Moq;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using AuctionService.Controllers;

namespace AuctionServiceTest
{
    public interface IMongoDbContextForTest
    {
        IMongoCollection<Auction> auctions { get; }
        IMongoDatabase GODatabase { get; }
        // Add other members if needed
    }

    public class MongoDBContext : IMongoDbContextForTest
    {
        public IMongoCollection<Auction> auctions { get; }
        public IMongoDatabase GODatabase { get; }
        // Implement other members
    }

    public class AuctionRepository
{
    private readonly IMongoDbContextForTest _mongoDbContext;
    private readonly ILogger<AuctionRepository> _logger;

    public AuctionRepository(IMongoDbContextForTest mongoDbContext, ILogger<AuctionRepository> logger)
    {
        _mongoDbContext = mongoDbContext ?? throw new ArgumentNullException(nameof(mongoDbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // Other methods...
}

    [TestFixture]
public class AuctionRepositoryTests
{
    private AuctionRepository _auctionRepository;
    private Mock<IMongoCollection<Auction>> _auctionsCollectionMock;
    private Mock<IMongoDbContextForTest> _mongoDbContextMock;
    private Mock<IMongoDatabase> _goDatabaseMock;
    private NLog.Logger _logger = null!;
    private IConfiguration _configuration = null!;

    [SetUp]
    public void Setup()
    {
        _auctionsCollectionMock = new Mock<IMongoCollection<Auction>>();
        _goDatabaseMock = new Mock<IMongoDatabase>();
        _goDatabaseMock.Setup(db => db.GetCollection<Auction>("auctions", null)).Returns(_auctionsCollectionMock.Object);

        _mongoDbContextMock = new Mock<IMongoDbContextForTest>();
        _mongoDbContextMock.Setup(m => m.auctions).Returns(_auctionsCollectionMock.Object);
        _mongoDbContextMock.Setup(m => m.GODatabase).Returns(_goDatabaseMock.Object);

        var loggerMock = new Mock<ILogger<AuctionRepository>>();
        _auctionRepository = new AuctionRepository(_mongoDbContextMock.Object, loggerMock.Object);
    }




        [Test]
        public async Task PostAuctionServiceTest()
        {
            Customer customer = new Customer { Id = "1", Name = "Johnny Doey", Email = "j@gmail" };
            // Arrange
            var item = new Item
            {
                Id = "1",
                Title = "Chair",
                Description = "The best chair",
                Category = Category.Home,
                Condition = Condition.Good,
                Location = "Amsterdam",
                StartPrice = 10,
                AssesmentPrice = 20,
                Year = 2021,
                Status = Status.Registered,
                Customer = customer
            };

            Auction auction = new Auction { Id = "1", StartTime = DateTime.Now, EndTime = DateTime.Now, Status = AuctionStatus.Active, Type = AuctionType.Dutch, Item = item };


            // Act
            await _auctionRepository.PostAuction(auction);


            // Assert
            _auctionsCollectionMock.Verify(
                m => m.InsertOneAsync(It.Is<Auction>(actualAuction =>
                actualAuction.Id == auction.Id &&
                actualAuction.StartTime == auction.StartTime &&
                actualAuction.EndTime == auction.EndTime &&
                actualAuction.Status == auction.Status &&
                actualAuction.Type == auction.Type &&
                actualAuction.Item.Id == auction.Item.Id

                ), null, default),
                Times.Once, "InsertOneAsync should be called with the expected auction object."
        );





        }

    }
}*/