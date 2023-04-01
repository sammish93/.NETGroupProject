using Azure;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.Security;
using Hiof.DotNetCourse.V2023.Group14.ProxyService.Configuration;
using Hiof.DotNetCourse.V2023.Group14.ProxyService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Proxy.Tests;

public class ProxyControllerTest
{
    private Mock<HttpClient> _mockHttp;
    private Mock<IHttpClientFactory> _mockHttpFactory;
    private Mock<IOptions<ProxySettings>> _mockSettings;

    private void SetupMockObjects(String url)
    {
        _mockHttp = new Mock<HttpClient>();
        _mockHttpFactory = new Mock<IHttpClientFactory>();
        _mockSettings = new Mock<IOptions<ProxySettings>>();
        _mockSettings.Setup(x => x.Value).Returns(new ProxySettings { LoginVerification = url });
        _mockHttpFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(_mockHttp.Object);
    }

    [Fact]
    public async Task Verification_ReturnsOkResult_WhenVerificationSucceeds()
    {
        // Setup
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        SetupMockObjects("https://localhost:7021/api/1.0/login/verification");

        _mockHttp.Setup(c =>
        c.SendAsync(
            It.IsAny<HttpRequestMessage>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(response);


        var controller = new V1ProxyController(_mockHttpFactory.Object, _mockSettings.Object);
        var loginInfo = new V1LoginInfo("testaccount", "TestPassword123!");

        // Act
        var result = await controller.Verification(loginInfo);

        // Assert
        Assert.IsType<OkResult>(result);
    }


    [Fact]
    public async Task Verification_ReturnsBadRequest_WhenVerificationFails()
    {
        // Setup
        var loginInfo = new V1LoginInfo("fail", "s");
        SetupMockObjects("https://localhost:7021/api/1.0/login/verification");

        _mockHttp.Setup(c => c.SendAsync(
            It.IsAny<HttpRequestMessage>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));

        var controller = new V1ProxyController(_mockHttpFactory.Object, _mockSettings.Object);

        // Act
        var result = await controller.Verification(loginInfo);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
    }

    // TODO: Fix the test, it does not work right now.
    [Fact]
    public async Task GetAll_ReturnsOkResult_WhenGetUsersSucceeds()
    {
        // Arrange
        var mockHttpFactory = new Mock<IHttpClientFactory>();
        var client = mockHttpFactory.Object.CreateClient();
        var mockClient = new Mock<HttpClient>();

        var settings = new Mock<IOptions<ProxySettings>>();
        var url = "https://localhost:7021/api/1.0/users/getUsers";

        mockHttpFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(mockClient.Object);

        settings.Setup(x => x.Value).Returns(new ProxySettings { GetUsers = url });

        var controller = new V1ProxyController(mockHttpFactory.Object, settings.Object);

        // Act
        var result = await controller.GetAll();

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

}
