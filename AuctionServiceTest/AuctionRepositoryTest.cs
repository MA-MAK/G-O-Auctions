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
        private Mock<MongoDBContext> _mongoDbContextMock;

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
            _mongoDbContextMock = new Mock<MongoDBContext>(MockBehavior.Strict, null, null, null);
            _mongoDbContextMock.Setup(m => m.Auctions).Returns(_auctionsCollectionMock.Object);
            _mongoDbContextMock.Setup(m => m.GODatabase).Returns(_goDatabaseMock.Object);
            // Create a mock of ILogger and IConfiguration
            var loggerMock = new Mock<ILogger<AuctionRepository>>();
            var configurationMock = new Mock<IConfiguration>();
            // Create AuctionRepository with the mocked MongoDBContext
            _auctionRepository = new AuctionRepository(_mongoDbContextMock.Object, loggerMock.Object, configurationMock.Object);
        }
        /*
        [SetUp]
        public void Setup()
        {
            // Create a mock of IMongoCollection<Auction>
            _auctionsCollectionMock = new Mock<IMongoCollection<Auction>>(MockBehavior.Strict);

            // Set up InsertOneAsync to return a completed task
            _auctionsCollectionMock
                .Setup(m => m.InsertOneAsync(It.IsAny<Auction>(), null, default))
                .Returns(Task.CompletedTask);

            // Create a mock of IMongoDatabase
            _goDatabaseMock = new Mock<IMongoDatabase>(MockBehavior.Strict);
            _goDatabaseMock.Setup(db => db.GetCollection<Auction>("auctions", null)).Returns(_auctionsCollectionMock.Object);

            // Create a mock of MongoDBContext using the mock database
            _mongoDbContextMock = new Mock<MongoDBContext>(MockBehavior.Strict, null, null, null);
            _mongoDbContextMock.Setup(m => m.Auctions).Returns(_auctionsCollectionMock.Object);
            _mongoDbContextMock.Setup(m => m.GODatabase).Returns(_goDatabaseMock.Object);

            // Create a mock of ILogger and IConfiguration
            var loggerMock = new Mock<ILogger<AuctionRepository>>();
            var configurationMock = new Mock<IConfiguration>();

            // Create AuctionRepository with the mocked MongoDBContext, ILogger, and IConfiguration
            _auctionRepository = new AuctionRepository(_mongoDbContextMock.Object, loggerMock.Object, configurationMock.Object);


        }
*/


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


            //await _auctionsCollectionMock.PostAuction(auction);

            _auctionsCollectionMock.Verify(
                m => m.InsertOneAsync(
                    It.Is<Auction>(a =>
                        a.Id == auction.Id &&
                        a.StartTime == auction.StartTime &&
                        a.EndTime == auction.EndTime &&
                        a.Status == auction.Status &&
                        a.Type == auction.Type &&
                        a.Item == auction.Item
                    ),
                    null,
                    default
                ),
                Times.Once
            );

        }/*
        [Test]
        public async Task PostAuctionServiceTest()
        {
            // Create a mock of IMongoCollection<Auction>
            _auctionsCollectionMock = new Mock<IMongoCollection<Auction>>();

            // Set up InsertOneAsync to return a completed task
            _auctionsCollectionMock
                .Setup(m => m.InsertOneAsync(It.IsAny<Auction>(), null, default))
                .Returns(Task.CompletedTask);

            // Create a mock of IMongoDatabase
            _goDatabaseMock = new Mock<IMongoDatabase>();
            _goDatabaseMock.Setup(db => db.GetCollection<Auction>("auctions", null)).Returns(_auctionsCollectionMock.Object);

            // Create a mock of MongoDBContext using the mock database
            _mongoDbContextMock = new Mock<MongoDBContext>(null, null, null);
            _mongoDbContextMock.Setup(m => m.Auctions).Returns(_auctionsCollectionMock.Object);
            _mongoDbContextMock.Setup(m => m.GODatabase).Returns(_goDatabaseMock.Object);

            // Create a mock of ILogger and IConfiguration
            var loggerMock = new Mock<ILogger<AuctionRepository>>();
            var configurationMock = new Mock<IConfiguration>();

            // Create AuctionRepository with the mocked MongoDBContext, ILogger, and IConfiguration
            _auctionRepository = new AuctionRepository(_mongoDbContextMock.Object, loggerMock.Object, configurationMock.Object);



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
            _auctionsCollectionMock.Verify(
                m => m.InsertOneAsync(
                      It.Is<Auction>(a =>
                        a.Id == auction.Id &&

                        a.Status == auction.Status &&
                        a.Type == auction.Type &&
                        a.Item == auction.Item 
                    ),
                    null,
                    default
                ),
                Times.Once
            );
        }

    } */
    } }
