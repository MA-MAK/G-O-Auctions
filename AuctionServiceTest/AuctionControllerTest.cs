using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Moq;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client.Exceptions;

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
         //Test if we can get an auction by ID - Use mock objects for ILogger and IConfiguration
       
        Assert.Pass();
    }

    

}