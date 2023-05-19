
using System.Data.Common;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.CommentService.Data;
using Hiof.DotNetCourse.V2023.Group14.CommentService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Runtime.CompilerServices;
using Grpc.Core;
using Hiof.DotNetCourse.V2023.Group14.CommentService;
using System;

namespace CommentServiceTests
{
    public class CommentServiceControllerTest
    {
        private CommentServiceContext _context;
        private Mock<ILogger<CommentService>> _loggerMock;
        private ServerCallContext _serverCallContext;

        private CommentService _commentService;

        public CommentServiceControllerTest()
        {
            var options = new DbContextOptionsBuilder<CommentServiceContext>()
            .UseInMemoryDatabase(databaseName: "test_database")
            .Options;
            _context = new CommentServiceContext(options);
            _loggerMock = new Mock<ILogger<CommentService>>();
            _serverCallContext = new Mock<ServerCallContext>().Object;

            _commentService = new CommentService(_context, _loggerMock.Object);
        }


        [Fact]
        public async Task GetComment_ValidId_ReturnsComment()
        {
            

            var validCommentId = Guid.NewGuid().ToString(); 
            var request = new GetCommentRequest { Id = validCommentId };
           

            
            var commentEntity = new V1Comments { 
                Id = Guid.Parse(validCommentId),
                Body = "Sample comment", 
                CreatedAt = DateTime.UtcNow, 
                Upvotes = 10,
                AuthorId = new Guid(), 
                CommentType = (Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1.CommentType)CommentType.Book,
                ISBN10 = "1234567890", 
                ISBN13 = "1234567890123", 
                 };
            _context.Comments.Add(commentEntity);
            _context.SaveChanges();

           
            var result = await _commentService.GetComment(request, _serverCallContext);

          
            Assert.Equal(validCommentId, result.Id);
            Assert.Equal("Sample comment", result.Body);
            
        }
        [Fact]
        public async Task GetComment_InvalidId_ReturnsNotFound()
        {
         
            

            var invalidCommentId = "invalid-id";
            var request = new GetCommentRequest { Id = invalidCommentId };
            var serverCallContext = new Mock<ServerCallContext>().Object;

            
            var exception = await Assert.ThrowsAsync<RpcException>(() => 
                _commentService.GetComment(request, serverCallContext));

            
            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
            Assert.Contains("Invalid comment ID format", exception.Status.Detail);
        }

        [Fact]
        public async Task GetComment_NonExistingId_ThrowsRpcException()
        {
            
            var nonExistingCommentId = Guid.NewGuid().ToString("N");
            var request = new GetCommentRequest { Id = nonExistingCommentId };

            
            var exception = await Assert.ThrowsAsync<RpcException>(async () => await 
                _commentService.GetComment(request, _serverCallContext));

            Assert.Equal(StatusCode.NotFound, exception.StatusCode);
            Assert.Contains($"Comment with ID '{request.Id}' not found.", exception.Status.Detail);
        }

        /*
        [Fact]
        public async Task GetAllComments_ReturnsAllComments()
        {
           
            
            var request = new Empty();
            

            
            var commentEntities = new List<V1Comments>
            {
                new V1Comments
                {
                    Id = Guid.NewGuid(),
                    Body = "Comment 1",
                    CreatedAt = DateTime.UtcNow,
                    Upvotes = 5,
                    AuthorId = Guid.NewGuid(),
                    CommentType = (Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1.CommentType)CommentType.Book,
                    ISBN10 = "1234567890",
                    ISBN13 = "1234567890123",
            
                },
                new V1Comments
                {
                    Id = Guid.NewGuid(),
                    Body = "Comment 2",
                    CreatedAt = DateTime.UtcNow,
                    Upvotes = 10,
                    AuthorId = Guid.NewGuid(),
                    CommentType = (Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1.CommentType) CommentType.Book,
                    ISBN10 = "0987654321",
                    ISBN13 = "9876543210987",
                    UserId = Guid.NewGuid()
                }
            };
            _context.Comments.AddRange(commentEntities);
            _context.SaveChanges();

            
            var result = await _commentService.GetAllComments(request, _serverCallContext);

            
            Assert.Equal(commentEntities.Count, result.Comments.Count);
            foreach (var commentEntity in commentEntities)
            {
                var comment = result.Comments.FirstOrDefault(c => c.Id == commentEntity.Id.ToString());
                Assert.NotNull(comment);
                Assert.Equal(commentEntity.Body, comment.Body);
                
            }
        }
        */

        [Fact]
        public async Task GetAllComments_ExceptionThrown_ReturnsRpcException()
        {

            
            var request = new Empty();
                

            
            _context.Comments = null;

            
            await Assert.ThrowsAsync<RpcException>(async () => await 
                _commentService.GetAllComments(request, _serverCallContext));
        }
        [Fact]
        public async Task GetCommentsByUserId_ValidUserId_ReturnsFilteredComments()
        {
           
            
            var userId = Guid.NewGuid(); 
            var request = new GetCommentsByUserIdRequest { UserId = userId.ToString() };
            

            var commentEntities = new List<V1Comments>
            {
                new V1Comments
                {
                    Id = Guid.NewGuid(),
                    Body = "Comment 1",
                    CreatedAt = DateTime.UtcNow,
                    Upvotes = 5,
                    AuthorId = Guid.NewGuid(),
                    UserId = userId
                },
                new V1Comments
                {
                    Id = Guid.NewGuid(),
                    Body = "Comment 2",
                    CreatedAt = DateTime.UtcNow,
                    Upvotes = 10,
                    AuthorId = Guid.NewGuid(),
                    UserId = userId
                }
            };
            _context.Comments.AddRange(commentEntities);
            _context.SaveChanges();

            
            var result = await _commentService.GetCommentsByUserId(request, _serverCallContext);

            
            Assert.Equal(commentEntities.Count, result.Response.Count);
            foreach (var commentEntity in commentEntities)
            {
                var comment = result.Response.FirstOrDefault(c => c.Id == commentEntity.Id.ToString());
                Assert.NotNull(comment);
                Assert.Equal(commentEntity.Body, comment.Body);
                
            }
        }


        [Fact]
        public async Task GetCommentsByUserId_InvalidUserId_ThrowsRpcException()
        {
            
            var request = new GetCommentsByUserIdRequest { UserId = "1203-32345555-2" };
           
            var exception = await Assert.ThrowsAsync<RpcException>(async () => await 
                _commentService.GetCommentsByUserId(request, _serverCallContext));

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
            Assert.Contains("Invalid user ID format", exception.Status.Detail);
        }
        [Fact]
        public async Task GetCommentsByUserId_NonExistingUserId_ThrowsRpcException()
        {
           
            var request = new GetCommentsByUserIdRequest { UserId = "" };

            var exception = await Assert.ThrowsAsync<RpcException>(async () => await 
                _commentService.GetCommentsByUserId(request, _serverCallContext));

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
            Assert.Contains("User ID must be entered", exception.Status.Detail);
        }

        [Fact]
        public async Task GetCommentsByUserId_NoCommentsFound_ThrowsRpcException()
        {
    
            var request = new GetCommentsByUserIdRequest { UserId = Guid.NewGuid().ToString() };
            

            var exception = await Assert.ThrowsAsync<RpcException>(async () => await 
                _commentService.GetCommentsByUserId(request, _serverCallContext));

            Assert.Equal(StatusCode.NotFound, exception.StatusCode);
            Assert.Contains($"No comments found for {request.UserId.ToString()}", exception.Status.Detail);
        }

        /*
        [Fact]
        public async Task GetCommentsByISBN_ValidISBN_ReturnsFilteredComments()
        {
            
            var isbn = "1234567890"; 
            var request = new GetCommentsByISBNRequest { Isbn = isbn };
            var serverCallContext = new Mock<ServerCallContext>().Object;

            var commentEntities = new List<V1Comments>
            {
                new V1Comments
                {
                    Id = Guid.NewGuid(),
                    Body = "Comment 1",
                    CreatedAt = DateTime.UtcNow,
                    Upvotes = 5,
                    AuthorId = Guid.NewGuid(),
                    ISBN10 = isbn,
                    ISBN13 = "9781234567890"
                },
                new V1Comments
                {
                    Id = Guid.NewGuid(),
                    Body = "Comment 2",
                    CreatedAt = DateTime.UtcNow,
                    Upvotes = 10,
                    AuthorId = Guid.NewGuid(),
                    ISBN10 = isbn,
                    ISBN13 = "6547812356741"
                }
            };
            _context.Comments.AddRange(commentEntities);
            _context.SaveChanges();

            
            var result = await _commentService.GetCommentsByISBN(request, serverCallContext);

            
            Assert.Equal(commentEntities.Count, result.Response.Count);
            foreach (var commentEntity in commentEntities)
            {
                var comment = result.Response.FirstOrDefault(c => c.Id == commentEntity.Id.ToString());
                Assert.NotNull(comment);
                Assert.Equal(commentEntity.Body, comment.Body);
                
            }
        }
        */

        [Fact]
        public async Task GetCommentsByISBN_InvalidISBN_ThrowsRpcException()
        {
            
            var invalidIsbn = "123"; 
            var request = new GetCommentsByISBNRequest { Isbn = invalidIsbn };
            

            var exception = await Assert.ThrowsAsync<RpcException>(() => 
                _commentService.GetCommentsByISBN(request, _serverCallContext));

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
            Assert.Contains("ISBN must be 10 or 13 digits.", exception.Status.Detail);
        }

        /*
        [Fact]
        public async Task GetCommentsByISBN_NoCommentsFound_ThrowsRpcException()
        {
          
            var isbn = "1234567890"; 
            var request = new GetCommentsByISBNRequest { Isbn = isbn };
            
            var exception = await Assert.ThrowsAsync<RpcException>(() =>
                _commentService.GetCommentsByISBN(request, _serverCallContext));

            Assert.Equal(StatusCode.NotFound, exception.StatusCode);
            Assert.Contains("No comments for this book yet.", exception.Status.Detail);

        }
        */

        /*
        [Fact]
        public async Task GetCommentsByAuthorId_ValidAuthorId_ReturnsCommentListedResponse()
        {
            
            var authorId = Guid.NewGuid().ToString();
            var request = new GetCommentsByAuthorIdRequest { AuthorId = authorId };

            var commentEntities = new List<V1Comments>
            {
                new V1Comments
                {
                    Id = Guid.NewGuid(),
                    Body = "Comment 1",
                    CreatedAt = DateTime.UtcNow,
                    Upvotes = 5,
                    AuthorId = Guid.Parse(authorId),
                    
                    CommentType = (Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1.CommentType)CommentType.Book,
                    ISBN10 = "4697423479",
                    ISBN13 = "9632145874102"
                },
                new V1Comments
                {
                    Id = Guid.NewGuid(),
                    Body = "Comment 2",
                    CreatedAt = DateTime.UtcNow,
                    Upvotes = 10,
                    AuthorId = Guid.Parse(authorId),
                   
                    CommentType = (Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1.CommentType) CommentType.User,

                    UserId = Guid.Parse("9dd4a9ea-0c95-4c54-b4a5-b6bbda930269")
                }
            };
            _context.Comments.AddRange(commentEntities);
            await _context.SaveChangesAsync();

            
            var response = await _commentService.GetCommentsByAuthorId(request, _serverCallContext);

            
            Assert.NotNull(response);
            Assert.Equal(commentEntities.Count, response.Response.Count);
            foreach (var commentEntity in commentEntities)
            {
                var comment = response.Response.FirstOrDefault(c => c.Id == commentEntity.Id.ToString());
                Assert.NotNull(comment);
                Assert.Equal(commentEntity.Body, comment.Body);
                
            }
        }
        */

        [Fact]
        public async Task GetCommentsByAuthorId_EmptyAuthorId_ThrowsRpcException()
        {
            
            var request = new GetCommentsByAuthorIdRequest { AuthorId = "" };

            
            var exception = await Assert.ThrowsAsync<RpcException>(() => 
                _commentService.GetCommentsByAuthorId(request, _serverCallContext));

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
            Assert.Contains("Author ID must be provided.", exception.Status.Detail);
        }

        [Fact]
        public async Task GetCommentsByAuthorId_InvalidAuthorIdFormat_ThrowsRpcException()
        {
            
            var request = new GetCommentsByAuthorIdRequest { AuthorId = "invalid_author_id" };

            
            var exception = await Assert.ThrowsAsync<RpcException>(() => 
                _commentService.GetCommentsByAuthorId(request, _serverCallContext));

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
            Assert.Contains("Invalid author ID format", exception.Status.Detail);
        }

        [Fact]
        public async Task GetCommentsByAuthorId_NoCommentsFound_ThrowsRpcException()
        {
            
            var authorId = Guid.NewGuid().ToString();
            var request = new GetCommentsByAuthorIdRequest { AuthorId = authorId };

            
            var exception = await Assert.ThrowsAsync<RpcException>(() => 
                _commentService.GetCommentsByAuthorId(request, _serverCallContext));

            Assert.Equal(StatusCode.NotFound, exception.StatusCode);
            Assert.Contains($"no comments found for {authorId}", exception.Status.Detail);
        }
        [Fact]
        public async Task DeleteComment_ValidCommentId_DeletesComment()
        {
            // Arrange
            var commentId = Guid.NewGuid().ToString();
            var request = new DeleteCommentRequest { Id = commentId };

            var commentEntity = new V1Comments
            {
                Id = Guid.Parse(commentId),
                Body = "Comment 1",
                CreatedAt = DateTime.UtcNow,
                Upvotes = 5,
                AuthorId = Guid.NewGuid(),
                CommentType = (Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1.CommentType)CommentType.User,
                UserId = Guid.Parse("9dd4a9ea-0c95-4c54-b4a5-b6bbda930269")  
            };

            _context.Comments.Add(commentEntity);
            await _context.SaveChangesAsync();

         
            var response = await _commentService.DeleteComment(request, _serverCallContext);

            
            Assert.NotNull(response);
            Assert.Equal($"{commentId} successfully deleted!", response.Message);

            var deletedComment = await _context.Comments.FindAsync(Guid.Parse(commentId));
            Assert.Null(deletedComment);
        }
        [Fact]
        public async Task DeleteComment_InvalidCommentIdFormat_ThrowsRpcException()
        {
            
            var request = new DeleteCommentRequest { Id = "invalid_comment_id" };

            
            var exception = await Assert.ThrowsAsync<RpcException>(() => _commentService.DeleteComment(request, _serverCallContext));

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
            Assert.Contains("Invalid comment id format", exception.Status.Detail);
        }

        [Fact]
        public async Task DeleteComment_CommentNotFound_ThrowsRpcException()
        {
            
            var commentId = Guid.NewGuid().ToString();
            var request = new DeleteCommentRequest { Id = commentId };

            
            var exception = await Assert.ThrowsAsync<RpcException>(() => _commentService.DeleteComment(request, _serverCallContext));

            Assert.Equal(StatusCode.NotFound, exception.StatusCode);
            Assert.Contains("Comment not found", exception.Status.Detail);
        }

        [Fact]
        public async Task CreateUserComment_ValidRequest_CreatesComment()
        {
            // Arrange
            var request = new CreateCommentRequest
            {
                Body = "Test Comment",
                Upvotes = 5,
                AuthorId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString()
            };


            var response = await _commentService.CreateComment(request, _serverCallContext);


            Assert.NotNull(response);
            Assert.Equal($"{response.Message.Split(' ')[0]} added successfully!!", response.Message);

            var commentIdString = response.Message.Split(' ')[0];
            Assert.True(Guid.TryParse(commentIdString, out var commentId), $"Invalid comment ID format: {commentIdString}");

            //check if comment exists in the db
            var comment = await _context.Comments.FindAsync(commentId);
            Assert.NotNull(comment);
            Assert.Equal(request.Body, comment.Body);
            Assert.Equal(request.Upvotes, comment.Upvotes);
            Assert.Equal(Guid.Parse(request.AuthorId), comment.AuthorId);
            Assert.Equal(Guid.Parse(request.UserId), comment.UserId);
        }
        [Fact]
        public async Task CreateComment_MissingCommentBody_ThrowsRpcException()
        {
            
            var request = new CreateCommentRequest
            {
                Body = "",
                Upvotes = 5,
                AuthorId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString()
            };

            
            var exception = await Assert.ThrowsAsync<RpcException>(() => _commentService.CreateComment(request, _serverCallContext));

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
            Assert.Contains("The comment body is required", exception.Status.Detail);
        }
        /*
        [Fact]
        public async Task CreateComment_MissingAuthorIdOrUserId_ThrowsRpcException()
        {
            // Arrange
            var request = new CreateCommentRequest
            {
                Body = "Test Comment",
                Upvotes = 5,
                AuthorId = null,
                UserId = null 
            };

            // Act and Assert
            var exception = await Assert.ThrowsAsync<RpcException>(() => _commentService.CreateComment(request, _serverCallContext));

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
            Assert.Contains("The author ID and user ID are required", exception.Status.Detail);
        }
        */

        [Fact]
        public async Task CreateBookComment_ValidRequest_CreatesComment()
        {
            // Arrange
            var request = new CreateBookCommentRequest
            {
                
                Body = "Test Comment",
                Upvotes = 5,
                AuthorId = Guid.NewGuid().ToString(),
                ISBN10 = "1234567890",
                ISBN13 = "9781234567890"
            };

            
            var response = await _commentService.CreateBookComment(request, _serverCallContext);

            Assert.NotNull(response);
            Assert.Equal($"{response.Message.Split(' ')[0]} added successfully!!", response.Message);

           

        }
        [Fact]
        public async Task CreateBookComment_MissingBody_ThrowsRpcException()
        {
            
            var request = new CreateBookCommentRequest
            {
                Body = "",
                Upvotes = 5,
                AuthorId = Guid.NewGuid().ToString(),
                ISBN10 = "1234567890",
                ISBN13 = "9781234567890"
            };

            
            var exception = await Assert.ThrowsAsync<RpcException>(() => 
                _commentService.CreateBookComment(request, _serverCallContext));

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
            Assert.Contains("The comment body is required", exception.Status.Detail);
        }
        [Fact]
        public async Task CreateBookComment_MissingISBN_ThrowsRpcException()
        {
            var request = new CreateBookCommentRequest
            {
                Body = "Test Comment",
                Upvotes = 5,
                AuthorId = Guid.NewGuid().ToString(),
                ISBN10 = "",
                ISBN13 = ""
            };

            
            var exception = await Assert.ThrowsAsync<RpcException>(() => 
                _commentService.CreateBookComment(request, _serverCallContext));

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
            Assert.Contains("Either ISBN10 or ISBN13 must be provided", exception.Status.Detail);
        }
        [Fact]
        public async Task CreateBookComment_InvalidISBN10Format_ThrowsRpcException()
        {
            
            var request = new CreateBookCommentRequest
            {
                Body = "Test Comment",
                Upvotes = 5,
                AuthorId = Guid.NewGuid().ToString(),
                ISBN10 = "12345", // Invalid ISBN10 format
                ISBN13 = ""
            };

            
            var exception = await Assert.ThrowsAsync<RpcException>(() => 
                _commentService.CreateBookComment(request, _serverCallContext));

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
            Assert.Contains("ISBN must be 10 or 13 digits", exception.Status.Detail);
        }

        [Fact]
        public async Task CreateReplyComment_ValidRequest_CreatesComment()
        {
            
            var request = new CreateReplyCommentRequest
            {
                Body = "Test Comment",
                Upvotes = 5,
                AuthorId = Guid.NewGuid().ToString(),
                ParentCommentId = Guid.NewGuid().ToString()
            };

            
            var response = await _commentService.CreateReplyComment(request, _serverCallContext);

            
            Assert.NotNull(response);
            Assert.Equal($"{response.Message.Split(' ')[0]} added successfully!!", response.Message);

            
        }
        [Fact]
        public async Task CreateReplyComment_MissingBody_ThrowsRpcException()
        {
            
            var request = new CreateReplyCommentRequest
            {
                Body = "", 
                Upvotes = 5,
                AuthorId = Guid.NewGuid().ToString(),
                ParentCommentId = Guid.NewGuid().ToString()
            };

            
            var exception = await Assert.ThrowsAsync<RpcException>(() => 
                _commentService.CreateReplyComment(request, _serverCallContext));

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
            Assert.Contains("The comment body is required.", exception.Status.Detail);
        }

        [Fact]
        public async Task UpdateComment_ValidRequest_ReturnsUpdatedCommentResponse()
        {
            
            var commentId = Guid.NewGuid();
            var existingComment = new V1Comments
            {
                Id = commentId,
                Body = "Original Comment",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            };
            _context.Comments.Add(existingComment);
            await _context.SaveChangesAsync();

            var request = new UpdateCommentRequest
            {
                Id = commentId.ToString(),
                Body = "Updated Comment"
            };

           
            var response = await _commentService.UpdateComment(request, _serverCallContext);

            
            Assert.NotNull(response);
            Assert.Equal($"Successfully updated! Updated comment is: {request.Body}", response.Message);

            var updatedComment = await _context.Comments.FindAsync(commentId);
            Assert.NotNull(updatedComment);
            Assert.Equal(request.Body, updatedComment.Body);
            Assert.Equal(existingComment.CreatedAt, updatedComment.CreatedAt);
        }
        [Fact]
        public async Task UpdateComment_InvalidCommentIdFormat_ThrowsRpcException()
        {
            
            var request = new UpdateCommentRequest
            {
                Id = "1234_9765",
                Body = "Updated Comment"
            };

            
            var exception = await Assert.ThrowsAsync<RpcException>(() => _commentService.UpdateComment(request, _serverCallContext));

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
            Assert.Contains("Invalid comment id format", exception.Status.Detail);
        }
        [Fact]
        public async Task UpdateComment_MissingCommentBody_ThrowsRpcException()
        {
            
            var request = new UpdateCommentRequest
            {
                Id = Guid.NewGuid().ToString(),
                Body = "" 
            };

            
            var exception = await Assert.ThrowsAsync<RpcException>(() 
                    => _commentService.UpdateComment(request, _serverCallContext));

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
            Assert.Contains("The comment body is required", exception.Status.Detail);
        }

        [Fact]
        public async Task UpdateComment_CommentNotFound_ThrowsRpcException()
        {
            
            var request = new UpdateCommentRequest
            {
                Id = Guid.NewGuid().ToString(),
                Body = "Updated Comment"
            };

            
            var exception = await Assert.ThrowsAsync<RpcException>(() =>
                _commentService.UpdateComment(request, _serverCallContext));

            Assert.Equal(StatusCode.NotFound, exception.StatusCode);
            Assert.Contains("Comment not found", exception.Status.Detail);
        }
        [Fact]
        public async Task IncrementUpvotes_ValidRequest_IncrementsUpvotesAndReturnsResponse()
        {
            
            var commentId = Guid.NewGuid();
            var existingComment = new V1Comments
            {
                Id = commentId,
                Body = "Test Comment",
                CreatedAt = DateTime.UtcNow,
                Upvotes = 5
            };
            _context.Comments.Add(existingComment);
            await _context.SaveChangesAsync();

            var request = new IncrementUpvotesRequest
            {
                Id = commentId.ToString()
            };

            
            var response = await _commentService.IncrementUpvotes(request, _serverCallContext);

            
            Assert.NotNull(response);
            Assert.Equal($"Upvotes successfully incremented! Total upvotes is: {existingComment.Upvotes}.", response.Message);
            
        }

        [Fact]
        public async Task IncrementUpvotes_InvalidCommentIdFormat_ThrowsRpcException()
        {
            
            var request = new IncrementUpvotesRequest
            {
                Id = "123-654"
            };

            
            var exception = await Assert.ThrowsAsync<RpcException>(() => _commentService.IncrementUpvotes(request, _serverCallContext));
            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
            Assert.Contains("Invalid comment id format", exception.Status.Detail);
        }
        [Fact]
        public async Task IncrementUpvotes_CommentNotFound_ThrowsRpcException()
        {
            
            var request = new IncrementUpvotesRequest
            {
                Id = Guid.NewGuid().ToString()
            };

            
            var exception = await Assert.ThrowsAsync<RpcException>(() => _commentService.IncrementUpvotes(request, _serverCallContext));
            Assert.Equal(StatusCode.NotFound, exception.StatusCode);
            Assert.Contains("Comment not found", exception.Status.Detail);
        }
    }
}