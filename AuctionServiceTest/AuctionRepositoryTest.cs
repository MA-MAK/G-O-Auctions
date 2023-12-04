using NUnit.Framework;
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
    public class AuctionRepositoryTests
    {
        private AuctionRepository _auctionRepository;
        private Mock<IMongoCollection<Auction>> _auctionsCollectionMock;
        private Mock<MongoDBContext> _mongoDbContextMock;

        private Mock<IMongoDatabase> _goDatabaseMock;

        private NLog.Logger _logger = null!;

        private IConfiguration _configuration = null!;


     [SetUp]
        public void Setup()
        {

        var loggerMock = new Mock<NLog.Logger>();
        _logger = loggerMock.Object;

        var configurationMock = new Mock<IConfiguration>();
        _configuration = configurationMock.Object;
        
        _configuration["MongoDBSettings:MongoConnectionString"] = "mongodb://localhost:27017";
    
    
        // Create a mock of IMongoCollection<Auction>
        _auctionsCollectionMock = new Mock<IMongoCollection<Auction>>(MockBehavior.Strict);

        // Create a mock of IMongoDatabase
        _goDatabaseMock = new Mock<IMongoDatabase>(MockBehavior.Strict);
        _goDatabaseMock.Setup(db => db.GetCollection<Auction>("auctions", null)).Returns(_auctionsCollectionMock.Object);

        // Create a mock of IMongoDBContext using the mock database
        // _mongoDbContextMock = new Mock<MongoDBContext>(MockBehavior.Strict);
       // _mongoDbContextMock.Setup(m => m.auctions).Returns(_auctionsCollectionMock.Object);
       // _mongoDbContextMock.Setup(m => m.GODatabase).Returns(_goDatabaseMock.Object);
       MongoDBContext mongoDbContext = new MongoDBContext(loggerMock.Object, configurationMock.Object);

        // Create AuctionRepository with the mocked IMongoDBContext
        _auctionRepository = new AuctionRepository(_mongoDbContextMock.Object);
        }

        [Test]
        public async Task PostAuctionServiceTest()
        {
            // Arrange
            var item = new Item
            {
                Id = 1,
                Title = "Chair",
                Description = "The best chair",
                Category = Category.Home,
                Condition = Condition.Good,
                Location = "Amsterdam",
                Seller = new Customer
                {
                    Id = 1,
                    Name = "Johnny Doey",
                    Email = "j@gmail"
                },
                StartPrice = 10,
                AssesmentPrice = 20,
                Year = 2021,
                Status = Status.Registered
            };

            Auction auction = new Auction { Id = 1, StartTime = DateTime.Now, EndTime = DateTime.Now, Status = AuctionStatus.Active, Type = AuctionType.Dutch, Item = item };

            // Act
            await _auctionRepository.PostAuction(auction);

            // Assert
            _auctionsCollectionMock.Verify(m => m.InsertOneAsync(It.IsAny<Auction>(), null, default), Times.Once);

        }
    }
}
