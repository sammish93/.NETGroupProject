﻿using Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Controllers;
using Moq;
namespace MarketplaceTests;

using Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Services;
using Microsoft.Extensions.Logging;

public class MarketPlaceControllerTest
{
    // Setup and mock objects. 
    private readonly V1MarketplaceController _controller;
    private readonly Mock<ILogger<V1MarketplaceController>> _loggerMock;
    private readonly Mock<V1MarketplaceService> _serviceMock;

    // Initialize the objects.
    public MarketPlaceControllerTest()
    {
        _loggerMock = new Mock<ILogger<V1MarketplaceController>>();
        _serviceMock = new Mock<V1MarketplaceService>();
        _controller = new V1MarketplaceController(_loggerMock.Object, _serviceMock.Object);
    }


    // TODO: Write first test.
}
