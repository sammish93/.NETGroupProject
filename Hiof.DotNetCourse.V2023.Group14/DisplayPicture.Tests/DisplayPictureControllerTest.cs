using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1;
using Hiof.DotNetCourse.V2023.Group14.UserDisplayPictureService.Controllers;
using Hiof.DotNetCourse.V2023.Group14.UserDisplayPictureService.Data;
using Hiof.DotNetCourse.V2023.Group14.UserDisplayPictureService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DisplayPicture.Tests;

public class DisplayPictureControllerTest
{
    // Tested on Mac - MAUI uses image deserialisation only available on Windows.



    private readonly V1DisplayPictureController _controller;
    private readonly Mock<V1UserIconService> _serviceMock;
    private readonly Mock<UserIconContext> _contextMock;

    public DisplayPictureControllerTest()
    {
        // In-memory database used for mocking purposes.
        var options = new DbContextOptionsBuilder<UserIconContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

        _contextMock = new Mock<UserIconContext>(options);
        _serviceMock = new Mock<V1UserIconService>(_contextMock.Object);
        _controller = new V1DisplayPictureController(_serviceMock.Object, _contextMock.Object);
    }

    [Fact]
    public async Task GetById_ReturnsBadRequestResult_WhenIdIsEmpty()
    {
        // Arrange.
        var id = Guid.Empty;

        // Act.
        var result = await _controller.GetById(id);

        // Assert.
        var badResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("ID parameter cannot be an emtpy guid!", badResult.Value);
    }

    /*
    [Fact]
    public async Task GetById_ReturnsNotFoundResult_WhenIdDoesNotExists()
    {
        // Arrange.
        var nonExistingId = Guid.NewGuid();
        _serviceMock.Setup(service => service.GetById(nonExistingId)).ReturnsAsync((V1UserIcon?)null);

        // Act.
        var result = await _controller.GetById(nonExistingId);

        // Assert.
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("User does not exists.", notFoundResult.Value);
    }
    */

    /*
    [Fact]
    public async Task GetById_ReturnsOkObjectResult_WhenIdIsValid()
    {
        // Arrange.
        var id = Guid.NewGuid();
        var icon = new V1UserIcon
        {
            Id = id,
            Username = "stian",
            DisplayPicture = new byte[] { 18, 52, 86, 120, 154 }
        };
        _serviceMock.Setup(service => service.GetById(id)).ReturnsAsync(icon);

        // Act.
        var result = await _controller.GetById(id);

        // Assert.
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        Assert.Equal(icon, okResult.Value);
    }
    */

    [Fact]
    public async Task GetByUsername_ReturnsBadRequest_WhenUsernameIsNullOrEmpty()
    {
        // Arrange.
        var username = null ?? "";

        // Act.
        var result = await _controller.GetByUsername(username);

        // Assert.
        var badResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Username parameter cannot be null or empty!", badResult.Value);
    }

    /*
    [Fact]
    public async Task GetByUsername_ReturnsNotFoundResult_IfUsernameDoesNotExists()
    {
        // Arrange.
        var username = "notExists";
        _serviceMock.Setup(service => service.GetByUsername(username)).ReturnsAsync((V1UserIcon?)null);

        // Act.
        var result = await _controller.GetByUsername(username);

        // Arrange.
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("User does not exists.", notFoundResult.Value);
    }
    */

    /*
    [Fact]
    public async Task GetByUsername_ReturnsOkObjectResult_WhenUsernameIsValid()
    {
        // Arrange.
        var username = "stian";
        var icon = new V1UserIcon
        {
            Id = Guid.NewGuid(),
            Username = username,
            DisplayPicture = new byte[] { 18, 52, 86, 120, 154 }
        };
        _serviceMock.Setup(service => service.GetByUsername(username)).ReturnsAsync(icon);

        // Act.
        var result = await _controller.GetByUsername(username);

        // Arrange
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        Assert.Equal(icon, okResult.Value);
    }
    */

    [Fact]
    public async Task Add_ReturnsBadRequest_WhenFileParameterIsNull()
    {
        // Arrange.
        var iconInputModel = GetModel(Guid.NewGuid(), "tester", null);

        // Act.
        var result = await _controller.Add(iconInputModel);

        // Assert.
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("File parameter cannot be null or empty.", badRequestResult.Value);
    }

    [Fact]
    public async Task Add_ReturnsBadRequest_WhenUsernameToShort()
    {
        // Arrange.
        var iconInputModel = GetModel(Guid.NewGuid(), "A", GetTestFile());

        // Act.
        var result = await _controller.Add(iconInputModel);

        // Assert.
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Username length must be between 5 - 15 characters.", badRequest.Value);
    }

    [Fact]
    public async Task Add_ReturnsBadRequest_WhenUsernameIsNullOrEmpty()
    {
        // Arrange.
        var iconInputModel = GetModel(Guid.NewGuid(), string.Empty, GetTestFile());

        // Act.
        var result = await _controller.Add(iconInputModel);

        // Assert.
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Username length must be between 5 - 15 characters.", badRequest.Value);
    }

    // TODO: Continue with test wrinting for the rest of the API.

    // Method used for testing purposes.
    private IFormFile GetTestFile()
    {
        var content = "this is a test file!";
        var fileName = "test.txt";
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(content);
        writer.Flush();
        ms.Position = 0;
        return new FormFile(ms, 0, ms.Length, null ?? "", fileName)
        {
            Headers = new HeaderDictionary(), ContentType = "text/plain"
        };
    }

    private V1AddIconInputModel GetModel(Guid id, string username, IFormFile file)
    {
        return new V1AddIconInputModel
        {
            Id = id,
            Username = username,
            File = file
        };
    }

}
