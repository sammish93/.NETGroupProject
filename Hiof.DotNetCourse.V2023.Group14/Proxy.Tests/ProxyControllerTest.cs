using Azure;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
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


    [Fact]
    public async Task GetAll_ReturnsContentResult_WhenGetUsersSucceeds()
    {
        // Setup
        var users = GetUserData();
        var baseAddress = "https://localhost:7021/api/1.0/users/getUsers";

        var mockHttp = new Mock<HttpMessageHandler>();
        var mockHttpFactory = new Mock<IHttpClientFactory>();
        var mockSettings = new Mock<IOptions<ProxySettings>>();

        mockSettings.Setup(x => x.Value).Returns(new ProxySettings { GetUsers = baseAddress });

        var httpClient = new HttpClient(mockHttp.Object)
        {
            BaseAddress = new Uri(baseAddress)
        };

        mockHttpFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Mock the response with the test data
        mockHttp.Protected().Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
        .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(users), Encoding.UTF8, "application/json")
        });

        var controller = new V1ProxyController(mockHttpFactory.Object, mockSettings.Object);

        // Act
        var result = await controller.GetAll();

        // Assert
        var contentResult = Assert.IsType<ContentResult>(result);
        var returnedUsersJson = contentResult.Content;
        var returnedUsers = JsonConvert.DeserializeObject<List<V1User>>(returnedUsersJson);
        Assert.NotNull(returnedUsers);
        Assert.Equal(users.Count, returnedUsers.Count);

    }

    private List<V1User> GetUserData()
    {
        List<V1User> userData = new List<V1User>
        {
            new V1User
            {
                Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                UserName = "JinkxMonsoon",
                Email = "joojoo@gmail.com",
                Password = "1674556689DE4173F6AE",
                FirstName = "Jinkx",
                LastName = "Monsoon",
                Country = "USA",
                City = "seattle",
                LangPreference = "en",
                Role = Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1.UserRole.Moderator,
                RegistrationDate = new DateTime(2023, 03, 05, 15, 58, 30),
                LastActive = new DateTime(2023, 03, 05, 15, 58, 30)
            },
            new V1User
            {
                Id = Guid.Parse("54af86bf-346a-4cba-b36f-527748e1cb93"),
                UserName = "testaccount",
                Email = "testme@test.no",
                Password = "A7E220F0781BE0C248A3",
                FirstName = "Ola",
                LastName = "Nordmann",
                Country = "Norway",
                City = "Oslo",
                LangPreference = "no",
                Role = Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1.UserRole.Admin,
                RegistrationDate = new DateTime(2023, 02, 24, 10, 42, 49),
                LastActive = new DateTime(2023, 02, 24, 10, 42, 49)
            },
            new V1User
            {
                Id = Guid.Parse("e8cc12ba-4df6-4b06-b96e-9ad00a927a93"),
                UserName = "QueenOfTheNorth",
                Email = "b_hyteso@gmail.com",
                Password = "B1A8A1223DCA3A102726",
                FirstName = "Brooklyn",
                LastName = "Hytes",
                Country = "Canada",
                City = "Toronto",
                LangPreference = "en",
                Role = Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1.UserRole.User,
                RegistrationDate = new DateTime(2023, 02, 24, 10, 39, 32),
                LastActive = new DateTime(2023, 02, 24, 10, 39, 32)
            }
        };

        return userData;
    }
}
