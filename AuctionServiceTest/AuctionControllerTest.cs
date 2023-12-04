using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using AuctionService.Controllers;
using Moq;
using Microsoft.AspNetCore.Mvc;
using AuctionService.Models;
using AuctionService.Services;

namespace AuctionServiceTest;

public class Tests
{
    private ILogger<AuctionController> _logger = null!;
    private IConfiguration _configuration = null!;

    [SetUp]
    public void Setup()
    {
        var loggerMock = new Mock<ILogger<AuctionController>>();
        _logger = loggerMock.Object;

        var configurationMock = new Mock<IConfiguration>();
        _configuration = configurationMock.Object;
    }

    [Test]
    public void GetAuctionByIDTest()
    {
        Customer customer = new Customer { Id = 1, Name = "Johnny Doey", Email = "j@gmail" };
        Customer bidder1 = new Customer { Id = 2, Name = "Mary Jane", Email = "m@gmail" };
        Customer bidder2 = new Customer { Id = 3, Name = "Peter Bennington", Email = "p@gmail" };
        Customer bidder3 = new Customer { Id = 4, Name = "Walter Leigh", Email = "w@gmail" };
        List<Bid> bids = new List<Bid>{
            new Bid { Id = 1, Bidder = bidder1, Amount = 1000, Time = DateTime.Now, AuctionId = 1 },
            new Bid { Id = 2, Bidder = bidder2, Amount = 2000, Time = DateTime.Now.AddMinutes(10), AuctionId = 1 },
            new Bid { Id = 3, Bidder = bidder3, Amount = 3000, Time = DateTime.Now.AddMinutes(30), AuctionId = 1 },
            new Bid { Id = 4, Bidder = bidder2, Amount = 5000, Time = DateTime.Now.AddMinutes(35), AuctionId = 1 }
        };

        Item item = new Item { Id = 1, Title = "Chair", Description = "The best chair", Category = Category.Home, Condition = Condition.Good, Location = "Amsterdam", Seller = customer, StartPrice = 10, AssesmentPrice = 20, Year = 2021, Status = Status.Registered, AuctionId = 1 };

        Auction auction = new Auction { Id = 1, StartTime = DateTime.Now, EndTime = DateTime.Now, Status = AuctionStatus.Active, Type = AuctionType.Dutch, Item = item };

        var ItemRepositoryMock = new Mock<IItemRepository>();
        ItemRepositoryMock.Setup(svc => svc.GetItemById(1))
            .Returns(Task.FromResult<Item?>(item));

        var BidRepositoryMock = new Mock<IBidRepository>();
        BidRepositoryMock.Setup(svc => svc.GetBidsForAuction(1))
            .Returns(Task.FromResult<IEnumerable<Bid>?>(bids));

        var AuctionRepositoryMock = new Mock<IAuctionRepository>();
        AuctionRepositoryMock.Setup(svc => svc.GetAuctionById(1))
            .Returns(Task.FromResult<Auction?>(auction));

        var controller = new AuctionController(_logger, _configuration, AuctionRepositoryMock.Object, ItemRepositoryMock.Object, BidRepositoryMock.Object);
        //Test if we can get an auction by ID - Use mock objects for ILogger and IConfiguration

        var result = controller.GetAuctionById(1).Result;

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        Assert.That((result as OkObjectResult)?.Value, Is.TypeOf<Auction>());
        Assert.That(((result as OkObjectResult)?.Value as Auction).Id, Is.EqualTo(1));
        Assert.That(((result as OkObjectResult)?.Value as Auction).Item, Is.TypeOf<Item>());
        Assert.That(((result as OkObjectResult)?.Value as Auction).Item.Id, Is.EqualTo(1));
        Assert.That(((result as OkObjectResult)?.Value as Auction).Bids[1], Is.TypeOf<Bid>());
        Assert.That(((result as OkObjectResult)?.Value as Auction).Bids.Count, Is.EqualTo(4));
        Assert.That(((result as OkObjectResult)?.Value as Auction).Bids[0].Id, Is.EqualTo(1));
    }

    [Test]
    public void PostAuctionTest()
    {
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

        var auction = new Auction
        {
            Id = 1,
            StartTime = DateTime.Now,
            EndTime = DateTime.Now,
            Status = AuctionStatus.Active,
            Type = AuctionType.Dutch,
            Item = item
        };

        var ItemRepositoryMock = new Mock<IItemRepository>();
        ItemRepositoryMock.Setup(svc => svc.GetItemById(1))
            .Returns(Task.FromResult<Item?>(item));

        var AuctionRepositoryMock = new Mock<IAuctionRepository>();
        AuctionRepositoryMock.Setup(svc => svc.PostAuction(auction))
            .Returns(Task.FromResult<Auction?>(auction));

        var controller = new AuctionController(_logger, _configuration, AuctionRepositoryMock.Object, ItemRepositoryMock.Object);

        var result = controller.PostAuction(auction).Result;

        // Assert
        Assert.That(result, Is.TypeOf<CreatedAtActionResult>());
        Assert.That((result as CreatedAtActionResult)?.Value, Is.TypeOf<Auction>());
        Assert.That(((result as CreatedAtActionResult)?.Value as Auction).Id, Is.EqualTo(1));
        Assert.That(((result as CreatedAtActionResult)?.Value as Auction).Item, Is.TypeOf<Item>());
        Assert.That(((result as CreatedAtActionResult)?.Value as Auction).Item.Id, Is.EqualTo(1));
    }

    [Test]
    public void PutAuctionTest()
    {
        // Arrange
        int id = 1;
        Auction auction = new Auction { Id = 1, Title = "Updated Auction", Description = "Updated Description" };
        var existingAuction = new Auction { Id = 1, Title = "Existing Auction", Description = "Existing Description" };
        var AuctionRepositoryMock = new Mock<IAuctionRepository>();
        AuctionRepositoryMock.Setup(svc => svc.GetAuctionById(id))
            .Returns(Task.FromResult<Auction?>(existingAuction));
        var ItemRepositoryMock = new Mock<IItemRepository>();
        var BidRepositoryMock = new Mock<IBidRepository>();
        var controller = new AuctionController(_logger, _configuration, AuctionRepositoryMock.Object, ItemRepositoryMock.Object, BidRepositoryMock.Object);

        // Act
        var result = controller.PutAuction(id, auction).Result;

        // Assert
        Assert.That(result, Is.TypeOf<NoContentResult>());
        Assert.That(existingAuction.Title, Is.EqualTo(auction.Title));
        Assert.That(existingAuction.Description, Is.EqualTo(auction.Description));
    }
}

/*
[Test]
public void PutAuctionTest()
{
    // Arrange
    int id = 1;
    Auction auction = new Auction { Id = 1, Title = "Updated Auction", Description = "Updated Description" };
    var existingAuction = new Auction { Id = 1, Title = "Existing Auction", Description = "Existing Description" };
    var auctions = new List<Auction> { existingAuction };
    var AuctionRepositoryMock = new Mock<IAuctionRepository>();
    AuctionRepositoryMock.Setup(svc => svc.GetAuctionById(id))
        .Returns(Task.FromResult<Auction?>(existingAuction));
    var controller = new AuctionController(_logger, _configuration, AuctionRepositoryMock.Object);


    // Act
    var result = controller.PutAuction(id, auction);

    // Assert
    Assert.That(result, Is.TypeOf<NoContentResult>());
    Assert.That(existingAuction.Title, Is.EqualTo(auction.Title));
    Assert.That(existingAuction.Description, Is.EqualTo(auction.Description));
}
*/