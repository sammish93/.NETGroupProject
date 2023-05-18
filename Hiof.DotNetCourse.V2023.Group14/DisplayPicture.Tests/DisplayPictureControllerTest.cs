using Hiof.DotNetCourse.V2023.Group14.UserDisplayPictureService.Controllers;
using Hiof.DotNetCourse.V2023.Group14.UserDisplayPictureService.Data;
using Hiof.DotNetCourse.V2023.Group14.UserDisplayPictureService.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DisplayPicture.Tests;

public class DisplayPictureControllerTest
{
    private readonly V1DisplayPictureController _controller;
    private readonly Mock<V1UserIconService> _serviceMock;
    private readonly Mock<UserIconContext> _contextMock;

    public DisplayPictureControllerTest()
    {
        _serviceMock = new Mock<V1UserIconService>();
        _contextMock = new Mock<UserIconContext>();
        _controller = new V1DisplayPictureController(_serviceMock.Object, _contextMock.Object);
    }

    [Fact]
    public async Task GetById_ReturnsBadRequestResult_WhenIdIsEmpty()
    {
        // Arrange.
        var id = Guid.Empty;
        _serviceMock.Setup(service => service.GetById(id));

        // Act.
    }
    
}
