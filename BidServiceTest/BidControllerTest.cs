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
    }

    [Test]
    public void GetBidsForAuctionTest()
    {
        Customer bidder1 = new Customer { Id = "1", Name = "Mary Jane", Email = "m@gmail" };
        Customer bidder2 = new Customer { Id = "2", Name = "Peter Bennington", Email = "p@gmail" };
        Customer bidder3 = new Customer { Id = "3", Name = "Walter Leigh", Email = "w@gmail" };
        List<Bid> bids = new List<Bid>{
            new Bid { Id = "1", Customer = bidder1, Amount = 1000, Time = DateTime.Now, AuctionId = "1"  },
            new Bid { Id = "2", Customer = bidder2, Amount = 2000, Time = DateTime.Now.AddMinutes(10), AuctionId = "1"  },
            new Bid { Id = "3", Customer = bidder3, Amount = 3000, Time = DateTime.Now.AddMinutes(30), AuctionId = "1"  },
            new Bid { Id = "4", Customer = bidder2, Amount = 5000, Time = DateTime.Now.AddMinutes(35), AuctionId = "1"  }
        };

        var BidRepositoryMock = new Mock<IBidRepository>();
        BidRepositoryMock.Setup(svc => svc.GetBidsForAuction("1"))
            .Returns(Task.FromResult<IEnumerable<Bid>?>(bids));

        var CustomerRepositoryMock = new Mock<ICustomerRepository>();
        CustomerRepositoryMock.Setup(svc => svc.GetCustomerById("1"))
            .Returns(Task.FromResult<Customer?>(bidder1));

        var controller = new BidController(BidRepositoryMock.Object, CustomerRepositoryMock.Object, _logger);

        var result = controller.GetBidsForAuction("1").Result;
        
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        Assert.That((result as OkObjectResult)?.Value, Is.TypeOf<List<Bid>>());
        Assert.That(((result as OkObjectResult)?.Value as List<Bid>)[1], Is.TypeOf<Bid>());
        Assert.That(((result as OkObjectResult)?.Value as List<Bid>).Count, Is.EqualTo(4));
        Assert.That(((result as OkObjectResult)?.Value as List<Bid>)[0].Id, Is.EqualTo("1"));
        Assert.That(((result as OkObjectResult)?.Value as List<Bid>)[0].Customer, Is.TypeOf<Customer>());
        Assert.That(((result as OkObjectResult)?.Value as List<Bid>)[0].Customer.Id, Is.EqualTo("1"));
    }
}