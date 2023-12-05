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
    [TestFixture]
    public class AuctionRepositoryTests
    {
        private AuctionRepository _auctionRepository;
        private Mock<IMongoCollection<Auction>> _auctionsCollectionMock;
        private Mock<IMongoDBContext> _mongoDbContextMock;

        private Mock<IMongoDatabase> _goDatabaseMock;

        private NLog.Logger _logger = null!;

        private IConfiguration _configuration = null!;


        [SetUp]
        public void Setup()
        {

            // Create a mock of IMongoCollection<Auction>
            _auctionsCollectionMock = new Mock<IMongoCollection<Auction>>(MockBehavior.Strict);

            // Create a mock of IMongoDatabase
            _goDatabaseMock = new Mock<IMongoDatabase>(MockBehavior.Strict);
            _goDatabaseMock.Setup(db => db.GetCollection<Auction>("auctions", null)).Returns(_auctionsCollectionMock.Object);

            // Create a mock of MongoDBContext using the mock database
            _mongoDbContextMock = new Mock<IMongoDBContext>(MockBehavior.Strict);
            _mongoDbContextMock.Setup(m => m.auctions).Returns(_auctionsCollectionMock.Object);
            _mongoDbContextMock.Setup(m => m.GODatabase).Returns(_goDatabaseMock.Object);
            // Create a mock of ILogger and IConfiguration
            var loggerMock = new Mock<ILogger<AuctionRepository>>();
            var configurationMock = new Mock<IConfiguration>();
            // Create AuctionRepository with the mocked MongoDBContext
            _auctionRepository = new AuctionRepository(_mongoDbContextMock.Object, loggerMock.Object, configurationMock.Object);
        }


        [Test]
        public async Task InsertAuctionServiceTest()
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

            Auction auction = new Auction { Id = "1", StartTime = DateTime.Now, EndTime = DateTime.Now, Status = AuctionStatus.Active, Type = AuctionType.Dutch, ItemId = 1 };


            // Act
            await _auctionRepository.InsertAuction(auction);


            // Assert
            _auctionsCollectionMock.Verify(
                m => m.InsertOneAsync(It.Is<Auction>(actualAuction =>
                actualAuction.Id == auction.Id  &&
                actualAuction.StartTime == auction.StartTime &&
                actualAuction.EndTime == auction.EndTime &&
                actualAuction.Status == auction.Status &&
                actualAuction.Type == auction.Type &&
                actualAuction.ItemId == auction.ItemId 
              
                ), null, default),
                Times.Once, "InsertOneAsync should be called with the expected auction object."
        );

    



        }

    }
}
