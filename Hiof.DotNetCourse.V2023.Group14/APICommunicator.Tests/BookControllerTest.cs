using Hiof.DotNetCourse.V2023.Group14.APICommunicatorService;
using Hiof.DotNetCourse.V2023.Group14.APICommunicatorService.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace APICommunicator.Tests;

public class BookControllerTest
{
    // TEST FOR GetBookISBN
    [Fact]
    public async Task GetValidResponeOnValidISBN()
    {
        var mockLogger = new Mock<ILogger<BookController>>();
        var controller = new BookController(mockLogger.Object);

        var ISBN = "1260440214";
        var result = await controller.Get(ISBN);

        var data = JsonSerializer.Deserialize<BookDTO>(result);
        string? id = data?.items?[0].id;

        Assert.Equal("YPRQtgEACAAJ", id);
    }

    [Fact]
    public async Task GetBadResponseOnShortISBN()
    {
        var mockLogger = new Mock<ILogger<BookController>>();
        var controller = new BookController(mockLogger.Object);

        var ISBN = "123";
        var result = await controller.Get(ISBN);

        Assert.Equal("ISBN must be 10 or 13 digits.", result);

    }

    [Fact]
    public async Task GetBadResponseOnFakeISBN()
    {
        var mockLogger = new Mock<ILogger<BookController>>();
        var controller = new BookController(mockLogger.Object);

        var ISBN = "6573849267364";
        var result = await controller.Get(ISBN);

        Assert.Equal("ISBN do not exists", result);

    }

    // TESTS FOR GetBookTitle
    [Fact]
    public async Task GetValidResponseOnValidTitle()
    {
        var mockLogger = new Mock<ILogger<BookController>>();
        var controller = new BookController(mockLogger.Object);

        var title = "DATABASESYSTEMER.";
        var result = await controller.GetByTitle(title);

        var data = JsonSerializer.Deserialize<BookDTO>(result);
        string? resultTitle = data?.items?[0].volumeInfo?.title;

        Assert.Equal(title, resultTitle);
    }


}
