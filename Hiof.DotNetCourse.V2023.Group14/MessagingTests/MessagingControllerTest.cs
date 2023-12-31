﻿using System.Diagnostics;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1;
using Hiof.DotNetCourse.V2023.Group14.MessagingService.Controllers;
using Hiof.DotNetCourse.V2023.Group14.MessagingService.Data;
using Hiof.DotNetCourse.V2023.Group14.MessagingService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
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
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task GetByParticipant_ReturnsNotFoundObjectResult_WhenParticipantNameDoesNotExists()
    {
        // Arrange
        var name = "byby";

        var controller = new V1MessagingController(_service);

        // Act
        var result = await controller.GetByParticipant(name);


        // Assert
        var okResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(404, okResult.StatusCode);
    }

    [Fact]
    public async Task CreateNewConversation_ReturnsOkObjectResult_WhenNewConversationIsCreated()
    {
        // Arrange
        var conversationId = new Guid("36c3b450-34bb-4ebf-9ad9-9eaab9d05be2");
        var controller = new V1MessagingController(_service);

        List<string> participants = new List<string> { "geir", "lars" };

        // Act
        var result = await controller.CreateNewConversation(conversationId, participants);

        // Asserts
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task CheckIfCreateNewConversation_CreatesNewConversation()
    {
        // Arrange
        var conversationId = new Guid("36c3b450-34bb-4ebf-9ad9-9eaab9d05be8");
        var controller = new V1MessagingController(_service);

        List<string> participants = new List<string> { "geir", "lars" };

        // Act
        var result = await controller.CreateNewConversation(conversationId, participants);

        // Check if the conversation was actually created
        var conversation = await _service.GetByConversationId(conversationId);
        Assert.NotNull(conversation);
        Assert.NotNull(conversation.Participants);
        Assert.Equal(conversationId, conversation.ConversationId);
    }

    [Fact]
    public async Task CreateNewConversation_ReturnsBadRequestObjectResult_OnEmptyId()
    {
        // Arrange
        var id = Guid.Empty;
        var controller = new V1MessagingController(_service);
        List<string> participants = new List<string> { "geir", "lars" };

        // Act
        var result = await controller.CreateNewConversation(id, participants);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequest.StatusCode);

    }

    [Fact]
    public async Task CreateNewConversation_ReturnsBadRequestObjectResult_WhenParticipantAreNull()
    {
        // Arrange
        var id = new Guid("26c5b440-34bb-4ebf-9ad9-9eaab9d05be8");
        var controller = new V1MessagingController(_service);

        // Act
        var result = await controller.CreateNewConversation(id, new List<string>());

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequest.StatusCode);

    }

    [Fact]
    public async Task CreateNewConversation_ReturnsBadRequestObjectResult_OnEmptyParticipantList()
    {
        // Arrange
        var id = new Guid("26c5b440-34bb-4ebf-9ad9-9eaab9d05be8");
        var controller = new V1MessagingController(_service);
        List<string> participants = new List<string> { };

        // Act
        var result = await controller.CreateNewConversation(id, participants);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequest.StatusCode);

    }


    [Fact]
    public async Task AddMessageToConversation_ReturnsOkObjectResult_WhenMessageAddedSuccessfully()
    {
        // Arrange
        var id = new Guid("36c3b450-34bb-4ebf-9ad9-9eaab9d05be8");
        var sender = "stian";
        var message = "hello everyone!";

        var controller = new V1MessagingController(_service);

        // Act
        var result = await controller.AddMessageToConversation(id, sender, message);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task AddMessageToConversation_ReturnsBadRequestObjectResult_WhenSenderIsNull()
    {
        // Arrange
        var id = new Guid("36c3b450-34bb-4ebf-9ad9-9eaab9d05be8");
        var message = "hello everyone!";

        var controller = new V1MessagingController(_service);

        // Act
        var result = await controller.AddMessageToConversation(id, null, message);

        // Assert
        var okResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, okResult.StatusCode);
    }

    [Fact]
    public async Task AddMessageToConversation_ReturnsBadRequestObjectResult_WhenMessageIsNull()
    {
        // Arrange
        var id = new Guid("36c3b450-34bb-4ebf-9ad9-9eaab9d05be8");
        var sender = "stian";

        var controller = new V1MessagingController(_service);

        // Act
        var result = await controller.AddMessageToConversation(id, sender, null);

        // Assert
        var okResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, okResult.StatusCode);
    }

    [Fact]
    public async Task AddMessageToConversation_ReturnsBadRequestObjectResult_WhenMessageLengthIsToBig()
    {
        // Arrange
        var id = new Guid("36c3b450-34bb-4ebf-9ad9-9eaab9d05be8");
        var sender = "stian";
        var message = "";

        // Message with a length of 130 characters. 
        for (int i = 0; i < 130; i++)
        {
            message += "a";
        }

        var controller = new V1MessagingController(_service);

        // Act
        var result = await controller.AddMessageToConversation(id, sender, message);

        // Assert
        var okResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, okResult.StatusCode);
    }

    [Fact]
    public async Task AddReactionToMessage_ReturnsOkObjectResult_OnSuccess()
    {
        // Arrange
        var id = Guid.Parse("36c3b450-34bb-4ebf-9ad9-9eaab9d05be8");
        var reaction = ReactionType.ANGRY;

        var controller = new V1MessagingController(_service);

        // Act
        var result = await controller.AddReactionToMessage(id, reaction);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task AddReactionToMessage_ReturnsBadRequestObjectResult_OnEmptyMessageId()
    {
        // Arrange
        var id = Guid.Empty;
        var reaction = ReactionType.ANGRY;

        var controller = new V1MessagingController(_service);

        // Act
        var result = await controller.AddReactionToMessage(id, reaction);

        // Assert
        var okResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, okResult.StatusCode);
    }

    [Fact]
    public async Task AddReactionToMessage_AddsReactionToAMessage()
    {

        // Arrange
        var id = Guid.Parse("36c3b450-34bb-4ebf-9ad9-9eaab9d05be8");

        var messages = new V1Messages
        {
            MessageId = id,
            Sender = "stian",
            Message = "Heyhey",
            Date = DateTime.UtcNow,
            Reactions = new List<V1Reactions>(),
            ConversationId = Guid.Parse("12c3b450-34bb-4ebf-9ad9-9eaab9d05be8")
        };

        var controller = new V1MessagingController(_service);

        await _dbContext.AddAsync(messages);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await controller.AddReactionToMessage(id, ReactionType.THUMBSUP);

        // Assert
        var message = await _dbContext.Messages.FindAsync(id);
        Assert.NotNull(message);
        Assert.Equal(ReactionType.THUMBSUP, message.Reactions[0].Type);
    }

    [Fact]
    public async Task UpdateMessage_ReturnsOkObjectResult_WhenValid()
    {
        // Arrange
        var messageId = Guid.Parse("012774ff-2d50-4f61-985a-4aa5da5fd8c4");
        var newMessage = "Hello World!";

        var message = new V1Messages
        {
            MessageId = messageId,
            Message = newMessage,
            Sender = "stian"
        };

        await _dbContext.Messages.AddAsync(message);
        await _dbContext.SaveChangesAsync();

        var controller = new V1MessagingController(_service);

        // Act
        var updatedResult = await controller.UpdateMessage(messageId, "New hello world!");
        var updatedMessage = await _dbContext.Messages.FindAsync(messageId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(updatedResult); 
        Assert.Equal("Message successfully updated!", okResult.Value);
        Assert.NotNull(updatedMessage);
        Assert.Equal("New hello world!", updatedMessage.Message);
    }

    [Fact]
    public async Task UpdateMessage_ReturnsBadObjectResult_OnInvalidParameter()
    {
        // Arrange
        var messageId = Guid.Parse("012774ff-2d50-4f61-985a-4aa5da5fd8c7");
        string newMessage = String.Empty;

        var controller = new V1MessagingController(_service);

        // Act
        var updatedResult = await controller.UpdateMessage(messageId, newMessage);

        // Assert
        var badResult = Assert.IsType<BadRequestObjectResult>(updatedResult);
        Assert.Equal(400, badResult.StatusCode);
    }

    [Fact]
    public async Task UpdateMessage_UpdatesMessage()
    {
        // Arrange
        var messageId = Guid.Parse("012774ff-2d50-4f61-985a-4aa5da5fd8c7");
        var newMessage = "Hello World!";

        var message = new V1Messages
        {
            MessageId = messageId,
            Sender = "sam",
            Message = "nothing special",
            Date = DateTime.UtcNow,
            ConversationId = Guid.Parse("16c3b450-37fb-4ebf-9ed9-9eeab9d05be8")
        };

        await _dbContext.AddAsync(message);
        await _dbContext.SaveChangesAsync();

        var controller = new V1MessagingController(_service);

        // Act
        var updatedResult = await controller.UpdateMessage(messageId, newMessage);
        var result = await _dbContext.Messages.FindAsync(messageId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Hello World!", result.Message.ToString());

    }

    [Fact]
    public async Task DeleteConversation_ReturnsOkObjectResult_OnSuccess()
    {
        // Arrange
        var conversationId = Guid.Parse("16c3b450-38fb-4ebf-9ed9-9eeab9d05de8");
        var controller = new V1MessagingController(_service);

        var conversation = new V1ConversationModel
        {
            ConversationId = conversationId,
        };

        await _dbContext.ConversationModel.AddAsync(conversation);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await controller.DeleteConversation(conversationId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);

    }

    [Fact]
    public async Task DeleteConversation_ReturnsBadRequestObject_OnBadId()
    {
        // Arrange
        Guid? conversationId = null;
        var controller = new V1MessagingController(_service);

        // Act
        var result = await controller.DeleteConversation(conversationId ?? Guid.Empty);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var badRequestObjectResult = (BadRequestObjectResult)result;
        Assert.Equal("Conversation with the ID does not exist", badRequestObjectResult.Value);
    }

    [Fact]
    public async Task DeleteConversation_DeletesConversationWithRightId()
    {
        // Arrange
        var conversationId = Guid.Parse("16c3b450-38fb-4ebf-9ed9-9eeab9d05de8");
        var controller = new V1MessagingController(_service);

        var conversation = new V1ConversationModel
        {
            ConversationId = conversationId
        };

        await _dbContext.ConversationModel.AddAsync(conversation);
        await _dbContext.SaveChangesAsync();

        // Check that it was actually added.
        Assert.NotNull(await _dbContext.ConversationModel.FindAsync(conversationId));

        // Act
        var result = await controller.DeleteConversation(conversationId);
        var deletedId = await _dbContext.ConversationModel.FindAsync(conversationId);

        // Assert
        Assert.Null(deletedId);
    }

    [Fact]
    public async Task DeleteMessage_ReturnsOkObjectResult_OnSuccess()
    {
        // Arrange
        var messageId = Guid.Parse("16c3b450-38fb-4ebf-9ed9-9eeab9d05de8");
        var controller = new V1MessagingController(_service);

        var message = new V1Messages
        {
            MessageId = messageId,
            Message = "hello hello",
            Sender = "stian"
        };

        await _dbContext.Messages.AddAsync(message);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await controller.DeleteMessage(messageId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task DeleteMessage_ReturnsBadObjectResult_OnFailure()
    {
        // Arrange
        Guid? messageId = null;
        var controller = new V1MessagingController(_service);

        // Act
        var result = await controller.DeleteMessage(messageId ?? Guid.Empty);

        // Assert
        var badResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badResult.StatusCode);
    }

    [Fact]
    public async Task DeleteMessage_DeletesMessage_WithRightId()
    {
        // Arrange
        var messageId = Guid.Parse("16c3b450-38fb-4ebf-9ed9-9eeab9d05de8");
        var controller = new V1MessagingController(_service);

        var message = new V1Messages
        {
            MessageId = messageId,
            Message = "hello test!",
            Sender = "stian"
        };

        await _dbContext.Messages.AddAsync(message);
        await _dbContext.SaveChangesAsync();

        // Check that message was added.
        Assert.NotNull(await _dbContext.Messages.FindAsync(messageId));

        // Act
        var result = await controller.DeleteMessage(messageId);

        var deletedMessage = await _dbContext.Messages.FindAsync(messageId);

        // Assert
        Assert.Null(deletedMessage);
    }
}
