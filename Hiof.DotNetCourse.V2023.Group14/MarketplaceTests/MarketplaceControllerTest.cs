using Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Controllers;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MarketplaceModels;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1;
using Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Data;
using Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
namespace MarketplaceTests;
using Moq;

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

    [Fact]
    public async Task GetPostByIsbn_ReturnsOkResult_WhenIsbnExists()
    {
        // Arrange.
        var bookResponse = responses;
        _serviceMock.Setup(service => service.GetPostByIsbn(bookResponse[0].ISBN10)).ReturnsAsync(bookResponse[0]);

        // Act.
        var result = await _controller.GetPostByIsbn("1627846358");

        // Arrange.
        var okResult = Assert.IsType<OkObjectResult>(result);
        var book = okResult.Value;
        Assert.Equal(responses[0], book);
    }

    [Fact]
    public async Task CreateNewPost_ReturnsBadRequestResults_WhenNoConditionIsProvided()
    {
        // Arrange.
        var book = new V1MarketplaceBook
        {
            Id = Guid.NewGuid(),
            Condition = "string",
            Price = 100,
            Currency = V1Currency.USD,
            Status = V1BookStatus.SOLD,
            OwnerId = Guid.NewGuid(),
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow,
            ISBN10 = "2364789098",
            ISBN13 = "2536478976453"
        };
        _serviceMock.Setup(service => service.CreateNewPost(book.OwnerId, book.Currency, book.Status, book)).ReturnsAsync(true);

        // Act.
        var result = await _controller.CreateNewPost(book.OwnerId, book.Currency, book.Status, book);

        // Assert.
        var badResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Please write about the condition to the book.", badResult.Value);
    }

    [Fact]
    public async Task CreateNewPost_ReturnsBadRequestResults_WhenNoPriceIsProvided()
    {
        // Arrange.
        var book = new V1MarketplaceBook
        {
            Id = Guid.NewGuid(),
            Condition = "good",
            Price = 0,
            Currency = V1Currency.USD,
            Status = V1BookStatus.SOLD,
            OwnerId = Guid.NewGuid(),
        };
        _serviceMock.Setup(service => service.CreateNewPost(book.OwnerId, book.Currency, book.Status, book)).ReturnsAsync(true);

        // Act.
        var result = await _controller.CreateNewPost(book.OwnerId, book.Currency, book.Status, book);

        // Assert.
        var badResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Need to set a price one the book!", badResult.Value);
    }

    [Fact]
    public async Task CreateNewPost_ReturnsBadRequestResults_OnFailure()
    {
        // Arrange.
        var book = new V1MarketplaceBook
        {
            Id = Guid.NewGuid(),
            Condition = "good",
            Price = 1000,
            Currency = V1Currency.SEK,
            Status = V1BookStatus.ORDERED,
            OwnerId = Guid.NewGuid(),
        };
        _serviceMock.Setup(service => service.CreateNewPost(book.OwnerId, book.Currency, book.Status, book)).ReturnsAsync(false);

        // Act.
        var result = await _controller.CreateNewPost(book.OwnerId, book.Currency, book.Status, book);

        // Assert.
        var badResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Could not add the post.", badResult.Value);

    }

    [Fact]
    public async Task CreateNewPost_ReturnsOkResult_OnSuccess()
    {
        // Arrange.
        var ownerId = Guid.NewGuid();
        var currency = V1Currency.EUR;
        var status = V1BookStatus.UNSOLD;

        var book = new V1MarketplaceBook
        {
            Id = Guid.NewGuid(),
            Condition = "brand new!",
            Price = 100,
            ISBN10 = "2364789098",
            ISBN13 = "2536478976453"
        };

        _serviceMock.Setup(service => service.CreateNewPost(ownerId, currency, status, book)).ReturnsAsync(true);

        // Act.
        var result = await _controller.CreateNewPost(ownerId, currency, status, book);

        // Assert.
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("New post successfully added!", okResult.Value);

    }

    [Fact]
    public async Task UpdatePost_ReturnsNotFoundResult_WhenProvidedIdDoesNotExists()
    {
        // Arrange.
        var post = new V1MarketplaceBookUpdated
        {
            Id = Guid.NewGuid(),
            Condition = "string",
            Price = 0,
            Currency = V1Currency.USD,
            Status = V1BookStatus.SOLD,
            OwnerId = Guid.NewGuid(),
        };
        var wrongId = "The id does not exists, please provide a valid id.";
        _serviceMock.Setup(service => service.UpdatePost(post.Id, post)).ReturnsAsync(wrongId);

        // Act.
        var result = await _controller.UpdatePost(post.Id, post);

        // Assert.
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task UpdatePost_ReturnsBadRequestResult_WhenISBN10HasInvalidLength()
    {
        // Arrange.
        var postId = Guid.NewGuid();
        var post = new V1MarketplaceBookUpdated
        {
            ISBN10 = "123456"
        };

        // Act.
        var result = await _controller.UpdatePost(postId, post);

        // Assert.
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("ISBN10 needs to have a length of 10.", badRequestResult.Value);
    }

    [Fact]
    public async Task UpdatePost_ReturnsBadRequestResult_WhenISBN13HasInvalidLength()
    {
        // Arrange.
        var postId = Guid.NewGuid();
        var post = new V1MarketplaceBookUpdated
        {
            ISBN13 = "123456789012"
        };

        // Act.
        var result = await _controller.UpdatePost(postId, post);

        // Assert.
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("ISBN13 needs to have a length of 13.", badRequestResult.Value);
    }

    [Fact]
    public async Task UpdatePost_ReturnsBadRequestResult_WhenConditionIsInvalid()
    {
        // Arrange.
        var postId = Guid.NewGuid();
        var post = new V1MarketplaceBookUpdated
        {
            Condition = "string",
        };

        var successMessage = "Successfully updated the post!";
        _serviceMock.Setup(service => service.UpdatePost(postId, post)).ReturnsAsync(successMessage);

        // Act.
        var result = await _controller.UpdatePost(postId, post);

        // Assert.
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Please write about the condition of the book.", badRequestResult.Value);
    }

    [Fact]
    public async Task UpdatePost_ReturnsOkResult_WhenUpdateSucceeds()
    {
        // Arrange.
        var postId = Guid.NewGuid();
        var post = new V1MarketplaceBookUpdated
        {
            Condition = "almost new",
            Price = 999,
            Currency = V1Currency.NOK,
            Status = V1BookStatus.UNSOLD,
            ISBN10 = "1234567890",
            ISBN13 = "1234567890123"
        };

        var successMessage = "Successfully updated the post!"; 
        _serviceMock.Setup(service => service.UpdatePost(postId, post)).ReturnsAsync(successMessage);

        // Act.
        var result = await _controller.UpdatePost(postId, post);

        // Assert.
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(successMessage, okResult.Value);
    }

    [Fact]
    public async Task DeletePost_ReturnsOkResult_WhenDeleteSucceeds()
    {
        // Arrange.
        var postId = Guid.NewGuid();
        var successStatus = true;
        _serviceMock.Setup(service => service.DeletePost(postId)).ReturnsAsync(successStatus);

        // Act.
        var result = await _controller.DeletePost(postId);

        // Assert.
        var okResult = Assert.IsType<OkObjectResult>(result);
        var compare = $"Post with id: {postId} - successfully deleted!";
        Assert.Equal(compare, okResult.Value);

    }

    [Fact]
    public async Task DeletePost_NotFoundResult_WhenDeleteFails()
    {
        // Arrange.
        var postId = Guid.NewGuid();
        var failStatus = false;
        _serviceMock.Setup(service => service.DeletePost(postId)).ReturnsAsync(failStatus);

        // Act.
        var result = await _controller.DeletePost(postId);

        // Assert.
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var compare = "Provided id does not exist, please provide another one";
        Assert.Equal(compare, notFoundResult.Value);

    }

}
