using Hiof.DotNetCourse.V2023.Group14.APICommunicatorService;
using Hiof.DotNetCourse.V2023.Group14.APICommunicatorService.Controllers;
using Microsoft.Extensions.Logging;

namespace APICommunicator.Tests;

public class BookControllerTest
{
    [Fact]
    public async Task GetValidResponeOnValidISBN()
    {
      
        var controller = new BookController();
        var ISBN = "1260440214";

        var result = await controller.Get(ISBN);

        Assert.NotEmpty(result);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetBadResponseOnShortISBN()
    {
        var controller = new BookController();
        var ISBN = "123";

        var result = await controller.Get(ISBN);

        Assert.Equal("ISBN must be 10 or 13 digits.", result);

    }

    [Fact]
    public async Task GetBadResponseOnFakeISBN()
    {
        var controller = new BookController();
        var ISBN = "6573849267364";

        var result = await controller.Get(ISBN);

        Assert.Equal("ISBN do not exists", result);

    }

}
