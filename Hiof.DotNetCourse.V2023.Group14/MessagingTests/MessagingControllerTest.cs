using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1;
using Hiof.DotNetCourse.V2023.Group14.MessagingService.Controllers;
using Hiof.DotNetCourse.V2023.Group14.MessagingService.Data;
using Hiof.DotNetCourse.V2023.Group14.MessagingService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace MessagingTests;

public class MessagingControllerTest
{
    private readonly MessagingContext _dbContext;
    private readonly Mock<ILogger<V1MessagingService>> _mockLogger;
    private readonly V1MessagingService _service;

    public MessagingControllerTest()
    {
        var options = new DbContextOptionsBuilder<MessagingContext>()
         .UseInMemoryDatabase(databaseName: "TestDatabase")
         .Options;

        _dbContext = new MessagingContext(options);
        _mockLogger = new Mock<ILogger<V1MessagingService>>();
        _service = new V1MessagingService(_dbContext, _mockLogger.Object);
    }


    [Fact]
    public async Task GetByConversationId_ReturnsOkObjectResult_WhenValidId()
    {
        // Arrange
        Guid testId = new Guid("16c3b450-37fb-4ebf-9ed9-9eeab9d05be8");
        var entity = new V1ConversationModel
        {
            ConversationId = testId,
            Participants = new List<V1Participant>(),
            Messages = new List<V1Messages>()
        };

        _dbContext.Add(entity);
        _dbContext.SaveChanges();

        var controller = new V1MessagingController(_service);

        // Act
        var result = await controller.GetByConversationId(testId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task GetByConversationId_ReturnsNotFoundObjectResult_WhenInvalidId()
    {
        // Arrange
        Guid testId = new Guid("25b6b470-37fb-4ebf-9ed9-9eeab9d05be8");
        var controller = new V1MessagingController(_service);

        // Act
        var result = await controller.GetByConversationId(testId);

        // Assert
        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFound.StatusCode);
    }

    [Fact]
    public async Task GetByConversationId_ReturnsBadRequestObject_WhenIdIsNull()
    {
        // Arrange
        var testId = Guid.Empty;
        var controller = new V1MessagingController(_service);

        // Act
        var result = await controller.GetByConversationId(testId);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequest.StatusCode);
    }

    [Fact]
    public async Task GetByParticipant_ReturnsOkObjectResult_WhenParticipantNameIsValid()
    {
        // Arrange
        var name = "stian";
        var id = new Guid("26c3b450-37fb-4ebf-9ed9-9eeab9d05be8");

        var participant = new V1Participant
        {
            Participant = name,
            ConversationId = id
        };

        var conversation = new V1ConversationModel
        {
            ConversationId = id,
            Participants = new List<V1Participant>(),
            Messages = new List<V1Messages>()
        };

        conversation.Participants.Add(participant);

        _dbContext.Add(conversation);
        await _dbContext.SaveChangesAsync();

        var controller = new V1MessagingController(_service);

        // Act
        var result = await controller.GetByParticipant(name);


        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task GetByParticipant_ReturnsNotFoundObjectResult_WhenParticipantNameDoesNotExists()
    {
        // Arrange
        var name = "fakeName";

        var controller = new V1MessagingController(_service);

        // Act
        var result = await controller.GetByParticipant(name);


        // Assert
        var okResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, okResult.StatusCode);
    }
}
