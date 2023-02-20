using Hiof.DotNetCourse.V2023.Group14.APICommunicatorService;
using Hiof.DotNetCourse.V2023.Group14.APICommunicatorService.Controllers.V1;
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
        var mockLogger = new Mock<ILogger<BookController>>();
        var controller = new BookController(mockLogger.Object);

        var isbn = "1260440214";
        var result = await controller.Get(isbn);

        var data = JsonSerializer.Deserialize<BookDto>(result);
        string? id = data?.items?[0].id;

        Assert.Equal("YPRQtgEACAAJ", id);
    }

    [Fact]
    public async Task GetBadResponseOnShortIsbn()
    {
        var mockLogger = new Mock<ILogger<BookController>>();
        var controller = new BookController(mockLogger.Object);

        var isbn = "123";
        var result = await controller.Get(isbn);

        Assert.Equal("ISBN must be 10 or 13 digits.", result);

    }

    [Fact]
    public async Task GetBadResponseOnFakeIsbn()
    {
        var mockLogger = new Mock<ILogger<BookController>>();
        var controller = new BookController(mockLogger.Object);

        var isbn = "6573849267364";
        var result = await controller.Get(isbn);

        Assert.Equal("ISBN do not exists", result);

    }

    // Test for GetBookTitle.
    [Fact]
    public async Task GetValidResponseOnValidTitle()
    {
        var mockLogger = new Mock<ILogger<BookController>>();
        var controller = new BookController(mockLogger.Object);

        var title = "DATABASESYSTEMER.";
        var result = await controller.GetByTitle(title);

        var data = JsonSerializer.Deserialize<BookDto>(result);
        string? resultTitle = data?.items?[0].volumeInfo?.title;

        Assert.Equal(title, resultTitle);
    }

    [Fact]
    public async Task GetErrorMessageOnFakeTitle()
    {
        var mockLogger = new Mock<ILogger<BookController>>();
        var controller = new BookController(mockLogger.Object);

        var title = "dfkjekjffd";
        var result = await controller.GetByTitle(title);

        Assert.Equal("Title do not exists", result);
    }

    
    // Test for GetBookAuthor.
    [Fact]
    public async Task GetValidResponseOnValidAuthor()
    {
        var mockLogger = new Mock<ILogger<BookController>>();
        var controller = new BookController(mockLogger.Object);

        var author = "Eric Matthes";
        var result = await controller.GetByAuthors(author);

        var data = JsonSerializer.Deserialize<BookDto>(result);
        string? name = data?.items?[0].volumeInfo?.authors?[0];

        Assert.Equal(author, name);
    }

    [Fact]
    public async Task GetErrorMessageOnFakeAuthor()
    {
        var mockLogger = new Mock<ILogger<BookController>>();
        var controller = new BookController(mockLogger.Object);

        var author = "dfkjekjffd";
        var result = await controller.GetByAuthors(author);

        Assert.Equal("Author do not exists", result);
    }

    // Test for GetBookCategories.
    [Fact]
    public async Task GetValidResponseOnValidCategory()
    {
        var mockLogger = new Mock<ILogger<BookController>>();
        var controller = new BookController(mockLogger.Object);

        var category = "Programming";
        var result = await controller.GetBySubject(category);

        var data = JsonSerializer.Deserialize<BookDto>(result);
        string? categoryResult = data?.items?[0].volumeInfo?.categories?[0];

        Assert.Equal("Computers", categoryResult);
    }
   
    [Fact]
    public async Task GetErrorMessageOnFakeCategory()
    {
        var mockLogger = new Mock<ILogger<BookController>>();
        var controller = new BookController(mockLogger.Object);

        var category = "fdjksad";
        var result = await controller.GetBySubject(category);

        Assert.Equal("Category do not exists", result);
    }
}
