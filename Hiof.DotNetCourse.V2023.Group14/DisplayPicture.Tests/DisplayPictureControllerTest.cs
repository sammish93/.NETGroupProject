using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1;
using Hiof.DotNetCourse.V2023.Group14.UserDisplayPictureService.Controllers;
using Hiof.DotNetCourse.V2023.Group14.UserDisplayPictureService.Data;
using Hiof.DotNetCourse.V2023.Group14.UserDisplayPictureService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DisplayPicture.Tests;

public class DisplayPictureControllerTest
{
    private readonly V1DisplayPictureController _controller;
    private readonly Mock<V1IUserIcon> _serviceMock;
    private readonly Mock<UserIconContext> _contextMock;

    public DisplayPictureControllerTest()
    {
        // In-memory database used for mocking purposes.
        var options = new DbContextOptionsBuilder<UserIconContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

        _contextMock = new Mock<UserIconContext>(options);
        _serviceMock = new Mock<V1IUserIcon>();
        _controller = new V1DisplayPictureController(_serviceMock.Object, _contextMock.Object);
    }

    [Fact]
    public async Task GetById_ReturnsBadRequestResult_WhenIdIsEmpty()
    {
        // Arrange.
        var id = Guid.Empty;

        // Act.
        var result = await _controller.GetById(id);

        // Arrange.
        var badResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("ID parameter cannot be an emtpy guid!", badResult.Value);
    }

    [Fact]
    public async Task GetById_ReturnsNotFoundResult_WhenIdDoesNotExists()
    {
        // Arrange.
        var nonExistingId = Guid.NewGuid();
        _serviceMock.Setup(x => x.GetById(nonExistingId)).ReturnsAsync((V1UserIcon?)null);

        // Act.
        var result = await _controller.GetById(nonExistingId);

        // Arrange.
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("User does not exists.", notFoundResult.Value);
    }
    
}
