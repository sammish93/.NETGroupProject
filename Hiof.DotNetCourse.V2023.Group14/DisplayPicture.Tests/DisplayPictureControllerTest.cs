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
    private readonly Mock<V1UserIconService> _serviceMock;
    private readonly Mock<UserIconContext> _contextMock;

    public DisplayPictureControllerTest()
    {
        // In-memory database used for mocking purposes.
        var options = new DbContextOptionsBuilder<UserIconContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

        _contextMock = new Mock<UserIconContext>();
        _serviceMock = new Mock<V1UserIconService>(MockBehavior.Default, _contextMock.Object);
        _controller = new V1DisplayPictureController(_serviceMock.Object, _contextMock.Object);
    }

    [Fact]
    public async Task GetById_ReturnsBadRequestResult_WhenIdIsEmpty()
    {
        // Arrange.
        var id = Guid.Empty;
        _serviceMock.Setup(service => service.GetById(id));

        // Act.
        var result = await _controller.GetById(id);

        // Arrange.
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
}
