using Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Controllers;
using Moq;
namespace MarketplaceTests;

using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MarketplaceModels;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1;
using Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Data;
using Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

public class MarketPlaceControllerTest
{
    // Setup and mock objects. 
    private readonly V1MarketplaceController _controller;
    private readonly Mock<ILogger<V1MarketplaceController>> _loggerMock;
    private readonly Mock<V1IMarketplace> _serviceMock;
    private readonly Mock<MarketplaceContext> _contextMock;

    // Initialize the objects.
    public MarketPlaceControllerTest()
    {
        _loggerMock = new Mock<ILogger<V1MarketplaceController>>();
        _serviceMock = new Mock<V1IMarketplace>();
        _controller = new V1MarketplaceController(_loggerMock.Object, _serviceMock.Object);
    }

    // Dummy data.
    private readonly List<V1MarketplaceBookResponse> responses = new List<V1MarketplaceBookResponse>
    {
        new V1MarketplaceBookResponse
            {
                Id = Guid.NewGuid(),
                Condition = "used",
                Price = 350,
                Currency = V1Currency.NOK,
                Status = V1BookStatus.UNSOLD,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
                ISBN10 = "1627846358",
                ISBN13 = "4567362545387"
            },
            new V1MarketplaceBookResponse
            {
                Id = Guid.NewGuid(),
                Condition = "almost new",
                Price = 799,
                Currency = V1Currency.NOK,
                Status = V1BookStatus.UNSOLD,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
                ISBN10 = "1223844368",
                ISBN13 = "1567262555787"

            }
    };
        

    [Fact]
    public async Task GetAllPosts_ReturnsNotFoundResult_WhenNoPostsExist()
    {
        // Arrange.
        _serviceMock.Setup(service => service.GetAllPosts()).ReturnsAsync(new List<V1MarketplaceBookResponse>());

        // Act.
        var result = await _controller.GetAllPosts();

        // Assert.
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetAllPosts_ReturnsOkResult_WhenPostsExists()
    {
        // Arrange.
        var bookResponse = responses;
        _serviceMock.Setup(service => service.GetAllPosts()).ReturnsAsync(bookResponse);

        // Act.
        var result = await _controller.GetAllPosts();

        // Assert.
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnBookResponses = Assert.IsType<List<V1MarketplaceBookResponse>>(okResult.Value);
        Assert.Equal(2, returnBookResponses.Count);
    }

    [Fact]
    public async Task GetPostById_ReturnsNotFoundResult_WhenIdDoesNotExists()
    {
        // Arrange.
        Guid postId = Guid.NewGuid();
        _serviceMock.Setup(service => service.GetPostById(postId)).ReturnsAsync((V1MarketplaceBookResponse?)null);

        // Act.
        var result = await _controller.GetPostById(postId);

        // Assert.
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetPostById_ReturnsOkResult_WhenIdExists()
    {
        // Arrange.
        Guid postId = responses[0].Id;
        var bookResponse = responses[0];
        _serviceMock.Setup(service => service.GetPostById(postId)).ReturnsAsync(bookResponse);

        // Act.
        var result = await _controller.GetPostById(postId);

        // Arrange.
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(bookResponse, okResult.Value);
    }

    [Fact]
    public async Task GetPostByIsbn_ReturnsBadRequestResult_WhenIsbnLengthToShort()
    {
        // Arrange.
        var isbn = "23475684";
        _serviceMock.Setup(service => service.GetPostByIsbn(isbn)).ReturnsAsync((V1MarketplaceBookResponse?)null);

        // Act.
        var result = await _controller.GetPostByIsbn(isbn);

        // Assert.
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetPostByIsbn_ReturnsBadRequestResult_WhenIsbnLengthToLong()
    {
        // Arrange.
        var isbn = "2347568497354678253642749838";
        _serviceMock.Setup(service => service.GetPostByIsbn(isbn)).ReturnsAsync((V1MarketplaceBookResponse?)null);

        // Act.
        var result = await _controller.GetPostByIsbn(isbn);

        // Assert.
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetPostByIsbn_ReturnsNotFoundResult_WhenNoIsbnExists()
    {
        // Arrange.
        var nonExistingIsbn = "1111111111";
        _serviceMock.Setup(service => service.GetPostByIsbn(nonExistingIsbn)).ReturnsAsync((V1MarketplaceBookResponse?)null);

        // Act.
        var result = await _controller.GetPostByIsbn(nonExistingIsbn);

        // Assert.
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
