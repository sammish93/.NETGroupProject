using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.APICommunicatorService.Controllers.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Hiof.DotNetCourse.V2023.Group14.APICommunicatorService.Configuration;

namespace APICommunicator.Tests;

public class BookControllerTest
{
    
    // Test for GetBookISBN. To run tests in VS, go to 'Test' and select 'Run all Tests'.
    [Fact]
    public async Task GetValidResponeOnValidIsbn()
    {
        var mockLogger = new Mock<ILogger<V1BookController>>();
        var mockSettings = new Mock<IOptions<ApiCommunicatorSettings>>();
        var controller = new V1BookController(mockLogger.Object, mockSettings.Object);

        var isbn = "1260440214";
        var maxResult = 10;
        var langRestrict = "en";
        var result = await controller.Get(isbn);

        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task GetBadResponseOnShortIsbn()
    {
        var mockLogger = new Mock<ILogger<V1BookController>>();
        var mockSettings = new Mock<IOptions<ApiCommunicatorSettings>>();
        var controller = new V1BookController(mockLogger.Object, mockSettings.Object);

        var isbn = "123";
        var maxResult = 10;
        var langRestrict = "en";
        var result = await controller.Get(isbn);

        var badResponse = Assert.IsType<BadRequestObjectResult>(result);

        Assert.Equal("ISBN must be 10 or 13 digits.", badResponse.Value);

    }

    [Fact]
    public async Task GetBadResponseOnFakeIsbn()
    {
        var mockLogger = new Mock<ILogger<V1BookController>>();
        var mockSettings = new Mock<IOptions<ApiCommunicatorSettings>>();
        var controller = new V1BookController(mockLogger.Object, mockSettings.Object);

        var isbn = "6573849267364";
        var maxResult = 10;
        var langRestrict = "en";
        var result = await controller.Get(isbn);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);

        Assert.Equal("No book found.", notFound.Value);

    }

    // Test for GetBookTitle.
    [Fact]
    public async Task GetValidResponseOnValidTitle()
    {
        var mockLogger = new Mock<ILogger<V1BookController>>();
        var mockSettings = new Mock<IOptions<ApiCommunicatorSettings>>();
        var controller = new V1BookController(mockLogger.Object, mockSettings.Object);

        var title = "DATABASESYSTEMER.";
        var maxResult = 10;
        var langRestrict = "en";
        var result = await controller.GetByTitle(title, maxResult, langRestrict);

        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task GetErrorMessageOnFakeTitle()
    {
        var mockLogger = new Mock<ILogger<V1BookController>>();
        var mockSettings = new Mock<IOptions<ApiCommunicatorSettings>>();
        var controller = new V1BookController(mockLogger.Object, mockSettings.Object);

        var title = "dfkjekjffd";
        var maxResult = 10;
        var langRestrict = "en";
        var result = await controller.GetByTitle(title, maxResult, langRestrict);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);

        Assert.Equal("No book found.", notFound.Value);
    }

    
    // Test for GetBookAuthor.
    [Fact]
    public async Task GetValidResponseOnValidAuthor()
    {
        var mockLogger = new Mock<ILogger<V1BookController>>();
        var mockSettings = new Mock<IOptions<ApiCommunicatorSettings>>();
        var controller = new V1BookController(mockLogger.Object, mockSettings.Object);

        var author = "Eric Matthes";
        var maxResult = 10;
        var langRestrict = "en";
        var result = await controller.GetByAuthors(author, maxResult, langRestrict);

        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task GetErrorMessageOnFakeAuthor()
    {
        var mockLogger = new Mock<ILogger<V1BookController>>();
        var mockSettings = new Mock<IOptions<ApiCommunicatorSettings>>();
        var controller = new V1BookController(mockLogger.Object, mockSettings.Object);

        var author = "dfkjekjffd";
        var maxResult = 10;
        var langRestrict = "en";
        var result = await controller.GetByAuthors(author, maxResult, langRestrict);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);

        Assert.Equal("No book found.", notFound.Value);
    }
    
    // Test for GetBookCategories.
    [Fact]
    public async Task GetValidResponseOnValidCategory()
    {
        var mockLogger = new Mock<ILogger<V1BookController>>();
        var mockSettings = new Mock<IOptions<ApiCommunicatorSettings>>();
        var controller = new V1BookController(mockLogger.Object, mockSettings.Object);

        var category = "Programming";
        var maxResult = 10;
        var langRestrict = "en";
        var result = await controller.GetBySubject(category, maxResult, langRestrict);

        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(200, okResult.StatusCode);
    }
    
   
    [Fact]
    public async Task GetErrorMessageOnFakeCategory()
    {
        var mockLogger = new Mock<ILogger<V1BookController>>();
        var mockSettings = new Mock<IOptions<ApiCommunicatorSettings>>();
        var controller = new V1BookController(mockLogger.Object, mockSettings.Object);

        var category = "fdjksad";
        var maxResult = 10;
        var langRestrict = "en";
        var result = await controller.GetBySubject(category, maxResult, langRestrict);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);

        Assert.Equal("No book found.", notFound.Value);
    }
}
