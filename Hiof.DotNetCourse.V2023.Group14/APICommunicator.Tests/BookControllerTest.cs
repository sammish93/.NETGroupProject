using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.APICommunicatorService.Controllers.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace APICommunicator.Tests;

public class BookControllerTest
{
    // Test for GetBookISBN. To run tests in VS, go to 'Test' and select 'Run all Tests'.
    [Fact]
    public async Task GetValidResponeOnValidIsbn()
    {
        var mockLogger = new Mock<ILogger<V1BookController>>();
        var controller = new V1BookController(mockLogger.Object);

        var isbn = "1260440214";
        var result = await controller.Get(isbn);

        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task GetBadResponseOnShortIsbn()
    {
        var mockLogger = new Mock<ILogger<V1BookController>>();
        var controller = new V1BookController(mockLogger.Object);

        var isbn = "123";
        var result = await controller.Get(isbn);

        var badResponse = Assert.IsType<BadRequestObjectResult>(result);

        Assert.Equal("ISBN must be 10 or 13 digits", badResponse.Value);

    }

    [Fact]
    public async Task GetBadResponseOnFakeIsbn()
    {
        var mockLogger = new Mock<ILogger<V1BookController>>();
        var controller = new V1BookController(mockLogger.Object);

        var isbn = "6573849267364";
        var result = await controller.Get(isbn);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);

        Assert.Equal("No book found.", notFound.Value);

    }

    // Test for GetBookTitle.
    [Fact]
    public async Task GetValidResponseOnValidTitle()
    {
        var mockLogger = new Mock<ILogger<V1BookController>>();
        var controller = new V1BookController(mockLogger.Object);

        var title = "DATABASESYSTEMER.";
        var result = await controller.GetByTitle(title);

        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task GetErrorMessageOnFakeTitle()
    {
        var mockLogger = new Mock<ILogger<V1BookController>>();
        var controller = new V1BookController(mockLogger.Object);

        var title = "dfkjekjffd";
        var result = await controller.GetByTitle(title);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);

        Assert.Equal("No book found.", notFound.Value);
    }

    
    // Test for GetBookAuthor.
    [Fact]
    public async Task GetValidResponseOnValidAuthor()
    {
        var mockLogger = new Mock<ILogger<V1BookController>>();
        var controller = new V1BookController(mockLogger.Object);

        var author = "Eric Matthes";
        var result = await controller.GetByAuthors(author);

        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task GetErrorMessageOnFakeAuthor()
    {
        var mockLogger = new Mock<ILogger<V1BookController>>();
        var controller = new V1BookController(mockLogger.Object);

        var author = "dfkjekjffd";
        var result = await controller.GetByAuthors(author);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);

        Assert.Equal("No book found.", notFound.Value);
    }

    // Test for GetBookCategories.
    [Fact]
    public async Task GetValidResponseOnValidCategory()
    {
        var mockLogger = new Mock<ILogger<V1BookController>>();
        var controller = new V1BookController(mockLogger.Object);

        var category = "Programming";
        var result = await controller.GetBySubject(category);

        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(200, okResult.StatusCode);
    }
   
    [Fact]
    public async Task GetErrorMessageOnFakeCategory()
    {
        var mockLogger = new Mock<ILogger<V1BookController>>();
        var controller = new V1BookController(mockLogger.Object);

        var category = "fdjksad";
        var result = await controller.GetBySubject(category);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);

        Assert.Equal("No book found.", notFound.Value);
    }
}
