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
        var stubRepo = new Mock<IAuctionRepository>();
        stubRepo.Setup(svc => svc.GetAuctionById(1))
            .Returns(Task.FromException<Auction?>(new Exception()));
        var controller = new AuctionController(_logger, _configuration, stubRepo.Object);
        //Test if we can get an auction by ID - Use mock objects for ILogger and IConfiguration

        var result = controller.GetAuctionById(1);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        Assert.That((result as OkObjectResult)?.Value, Is.TypeOf<Auction>());
        //Assert.That((result as OkObjectResult)?.Value, Is.EqualTo(1));

        Assert.Pass();
    }



}