using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Moq;
using Microsoft.AspNetCore.Mvc;
using BidService.Controllers;
using BidService.Models;
using BidService.Services;

namespace BidServiceTest;

public class Tests
{
    private ILogger<BidController> _logger = null!;
    private IConfiguration _configuration = null!;

    [SetUp]
    public void Setup()
    {
        var loggerMock = new Mock<ILogger<BidController>>();
        _logger = loggerMock.Object;

        var configurationMock = new Mock<IConfiguration>();
        _configuration = configurationMock.Object;
    }

    [Test]
    public void GetBidsForAuctionTest()
    {
        Customer customer = new Customer { Id = 1, Name = "Johnny Doey", Email = "j@gmail" };
        Bid bid = new Bid { Id = 1, Bidder = customer, Amount = 1000, Time = DateTime.Now, AuctionId = 1 };

        var BidRepositoryMock = new Mock<IBidRepository>();
        BidRepositoryMock.Setup(svc => svc.GetBidsForAuction(1))
            .Returns(Task.FromResult<IEnumerable<Bid>?>(new List<Bid> { bid }));

        var CustomerRepositoryMock = new Mock<ICustomerRepository>();
        CustomerRepositoryMock.Setup(svc => svc.GetCustomerByForBid(1))
            .Returns(Task.FromResult<Customer?>(customer));

        var controller = new BidController(_logger, _configuration, BidRepositoryMock.Object);

        var result = controller.GetBidsForAuction(1);
        
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        Assert.That((result as OkObjectResult)?.Value, Is.TypeOf<List<Bid>>());
        Assert.That(((result as OkObjectResult)?.Value as List<Bid>)[1], Is.TypeOf<Bid>());
        Assert.That(((result as OkObjectResult)?.Value as List<Bid>).Count, Is.EqualTo(4));
        Assert.That(((result as OkObjectResult)?.Value as List<Bid>)[0].Id, Is.EqualTo(1));
        Assert.That(((result as OkObjectResult)?.Value as List<Bid>)[0].Bidder, Is.TypeOf<Customer>());
        Assert.That(((result as OkObjectResult)?.Value as List<Bid>)[0].Bidder.Id, Is.EqualTo(1));
    }
}