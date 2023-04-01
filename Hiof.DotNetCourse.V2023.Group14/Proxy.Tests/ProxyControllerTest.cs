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

namespace Proxy.Tests;

public class ProxyControllerTest
{

    [Fact]
    public async Task Verification_ReturnsOkResult_WhenVerificationSucceeds()
    {
        // Arrange
        var mockHttp = new Mock<HttpClient>();
        var mockHttpFactory = new Mock<IHttpClientFactory>();
        var mockSettings = new Mock<IOptions<ProxySettings>>();
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        var url = "https://localhost:7021/api/1.0/login/verification";

        mockHttpFactory.Setup(f =>
        f.CreateClient(It.IsAny<string>())).Returns(mockHttp.Object);

        mockHttp.Setup(c =>
        c.SendAsync(
            It.IsAny<HttpRequestMessage>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(response);

        mockSettings.Setup(s =>
        s.Value).Returns(new ProxySettings{ LoginVerification = url });

        var controller = new V1ProxyController(mockHttpFactory.Object, mockSettings.Object);
        var loginInfo = new V1LoginInfo("testaccount", "TestPassword123!");

        // Act
        var result = await controller.Verification(loginInfo);

        // Assert
        Assert.IsType<OkResult>(result);
    }


    [Fact]
    public async Task Verification_ReturnsBadRequest_WhenVerificationFails()
    {
        // Arrange
        var mockHttp = new Mock<HttpClient>();
        var mockSettings = new Mock<IOptions<ProxySettings>>();
        var loginInfo = new V1LoginInfo("fail", "s");
        var url = "https://localhost:7021/api/1.0/login/verification";

        mockHttp.Setup(c => c.SendAsync(
            It.IsAny<HttpRequestMessage>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));

        mockSettings.Setup(s => s.Value).Returns(new ProxySettings { LoginVerification = url });

        var controller = new V1ProxyController(Mock.Of<IHttpClientFactory>(), mockSettings.Object);

        // Act
        var result = await controller.Verification(loginInfo);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
    }

}
