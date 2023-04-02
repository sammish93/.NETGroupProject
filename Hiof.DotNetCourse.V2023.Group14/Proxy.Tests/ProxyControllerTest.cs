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
    private readonly V1ProxyController _controller;
    private readonly Mock<HttpMessageHandler> _mockHttp;
    private readonly Mock<IHttpClientFactory> _mockHttpFactory;
    private readonly Mock<IOptions<ProxySettings>> _mockSettings;

    public ProxyControllerTest()
    {
        _mockHttp = new Mock<HttpMessageHandler>();
        _mockHttpFactory = new Mock<IHttpClientFactory>();
        _mockSettings = new Mock<IOptions<ProxySettings>>();

        var httpClient = new HttpClient(_mockHttp.Object);
        _mockHttpFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
        _controller = new V1ProxyController(_mockHttpFactory.Object, _mockSettings.Object);
    }

    private void SetupProxySettings(ProxySettings settings)
    {
        _mockSettings.Setup(x => x.Value).Returns(settings);
    }

    [Fact]
    public async Task Verification_ReturnsOkResult_WhenVerificationSucceeds()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        var url = "https://localhost:7021/api/1.0/login/verification";

        SetupProxySettings(new ProxySettings { LoginVerification = url });

        _mockHttp.Protected().Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var loginInfo = new V1LoginInfo("testaccount", "TestPassword123!");

        // Act
        var result = await _controller.Verification(loginInfo);

        // Assert
        Assert.IsType<OkResult>(result);
    }


    [Fact]
    public async Task Verification_ReturnsBadRequest_WhenVerificationFails()
    {
        // Arrange
        var loginInfo = new V1LoginInfo("fail", "s");
        var url = "https://localhost:7021/api/1.0/login/verification";

        SetupProxySettings(new ProxySettings { LoginVerification = url });

        _mockHttp.Protected().Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));

        // Act
        var result = await _controller.Verification(loginInfo);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
    }


    [Fact]
    public async Task GetAll_ReturnsContentResult_WhenGetUsersSucceeds()
    {
        // Setup
        var users = GetUserData();
        var url = "https://localhost:7021/api/1.0/users/getUsers";

        SetupProxySettings(new ProxySettings { GetUsers = url });

        _mockHttp.Protected().Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
        .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(users), Encoding.UTF8, "application/json")
        });


        // Act
        var result = await _controller.GetAll();

        // Assert
        var contentResult = Assert.IsType<ContentResult>(result);
        var returnedUsersJson = contentResult.Content;
        var returnedUsers = JsonConvert.DeserializeObject<List<V1User>>(returnedUsersJson);

        Assert.NotNull(returnedUsers);
        Assert.Equal(users.Count, returnedUsers.Count);
    }

  

    [Fact]
    public async Task GetAll_ReturnsNotFound_WhenGetAllFails()
    {
        // Arrange
        List<V1User> users = new List<V1User>();
        var url = "https://localhost:7021/api/1.0/users/getUsers";

        SetupProxySettings(new ProxySettings { GetUsers = url });

        _mockHttp.Protected().Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
        .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent(JsonConvert.SerializeObject(users), Encoding.UTF8, "application/json")
        });

        // Act
        var result = await _controller.GetAll();

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }


    [Fact]
    public async Task GetById_ReturnsContentResult_WhenGetByIdSucceeds()
    {
        // Arrange
        var userList = GetUserData();
        var id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
        var user = userList.FirstOrDefault(u => u.Id == id);
        var url = "https://localhost:7021/api/1.0/users/getUserById";

        SetupProxySettings(new ProxySettings { GetUserById = url });

        _mockHttp.Protected().Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.Is<HttpRequestMessage>(x => x.RequestUri != null && x.RequestUri.ToString().Contains(id.ToString())),
            ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")
            });

        // Act
        var result = await _controller.GetById(id);

        var contentResult = Assert.IsType<ContentResult>(result);
        var returnedJson = contentResult.Content;
        var returnedUser = JsonConvert.DeserializeObject<V1User>(returnedJson);

        // Assert
        Assert.NotNull(returnedUser);
        Assert.NotNull(user);

        Assert.Equal(id, returnedUser.Id);
        Assert.Equal(user.UserName, returnedUser.UserName);
        Assert.Equal(user.Email, returnedUser.Email);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenGetByIdFails()
    {
        // Arrange
        var userList = GetUserData();

        // Fake random id.
        var id = Guid.Parse("3fa85f64-5717-4572-b3fc-2c863f66afb6"); 
        var user = userList.FirstOrDefault(u => u.Id == id);
        var url = "https://localhost:7021/api/1.0/users/getUserById";

        SetupProxySettings(new ProxySettings { GetUserById = url });

        _mockHttp.Protected().Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.Is<HttpRequestMessage>(x => x.RequestUri != null && x.RequestUri.ToString().Contains(id.ToString())),
            ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound));

        // Act
        var result = await _controller.GetById(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);

    }
    

    // Dummy data used for testing.
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
