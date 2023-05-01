using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using AutoMapper;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Hiof.DotNetCourse.V2023.Group14.CommentService.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Hiof.DotNetCourse.V2023.Group14.CommentService.Services
{
    public class CommentService : CommentingService.CommentingServiceBase
    {
        private readonly ILogger<CommentService> _logger;
        private readonly CommentServiceContext _context;


        public CommentService(CommentServiceContext context, ILogger<CommentService> logger)
        {
            _context = context;
            _logger = logger;

        }

        public override Task<Comment> GetComment(GetCommentRequest request, ServerCallContext context)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Id))
                {
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Comment ID must be provided."));
                }

                if (!Guid.TryParse(request.Id, out var commentId))
                {
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid comment ID format."));
                }

                var commentEntity = _context.Comments.Include(c => c.Replies).FirstOrDefault(c => c.Id == commentId);

                if (commentEntity == null)
                {
                    _logger.LogWarning("{StatusCode}Comment with ID {Id} not found", StatusCode.NotFound, request.Id);

                    throw new RpcException(new Status(StatusCode.NotFound, $"Comment with ID '{request.Id}' not found."));
                }

                var response = new Comment
                {
                    Id = commentEntity.Id.ToString(),
                    Body = commentEntity.Body,
                    CreatedAt = Timestamp.FromDateTime(DateTime.SpecifyKind(commentEntity.CreatedAt, DateTimeKind.Utc)),
                    Upvotes = commentEntity.Upvotes ?? 0,
                    AuthorId = commentEntity.AuthorId.ToString(),
                    ParentCommentId = commentEntity.ParentCommentId?.ToString() ?? "",
                    CommentType = (CommentType)commentEntity.CommentType,
                    ISBN10 = commentEntity.ISBN10 ?? "",
                    ISBN13 = commentEntity.ISBN13 ?? "",
                    UserId = commentEntity.UserId?.ToString() ?? ""
                };

                foreach (var replyEntity in commentEntity.Replies)
                {
                    var reply = new Comment
                    {
                        Id = replyEntity.Id.ToString(),
                        Body = replyEntity.Body,
                        CreatedAt = Timestamp.FromDateTime(DateTime.SpecifyKind(replyEntity.CreatedAt, DateTimeKind.Utc)),
                        Upvotes = replyEntity.Upvotes ?? 0,
                        AuthorId = replyEntity.AuthorId.ToString(),
                        ParentCommentId = replyEntity.ParentCommentId?.ToString() ?? "",
                        CommentType = (CommentType)replyEntity.CommentType,
                        ISBN10 = replyEntity.ISBN10 ?? "",
                        ISBN13 = replyEntity.ISBN13 ?? "",
                        UserId = replyEntity.UserId?.ToString() ?? ""
                    };

                    response.Replies.Add(reply);
                }

                return Task.FromResult(response);
            }
            catch (RpcException)
            {
                throw; // rethrow any existing RpcException
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while handling GetComment request");
                throw new RpcException(new Status(StatusCode.Internal, "An unexpected error occurred while processing the request."));
            }
        }


        public override async Task<CommentList> GetAllComments(Empty request, ServerCallContext context)
        {
            try
            {
                _logger.LogInformation("GetAllComments is called");
                var comments = await _context.Comments.ToListAsync();

                var commentList = new CommentList();

                foreach (var commentEntity in comments)
                {
                    var comment = new Comment
                    {
                        Id = commentEntity.Id.ToString(),
                        Body = commentEntity.Body,
                        CreatedAt = Timestamp.FromDateTime(DateTime.SpecifyKind(commentEntity.CreatedAt, DateTimeKind.Utc)),
                        Upvotes = commentEntity.Upvotes ?? 0,
                        AuthorId = commentEntity.AuthorId.ToString(),
                        ParentCommentId = commentEntity.ParentCommentId?.ToString() ?? "",
                        CommentType = (CommentType)commentEntity.CommentType,
                        ISBN10 = commentEntity.ISBN10 ?? "",
                        ISBN13 = commentEntity.ISBN13 ?? "",
                        UserId = commentEntity.UserId?.ToString() ?? ""
                    };

                    commentList.Comments.Add(comment);
                }

                return commentList;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while getting all comments.", ex.Message.ToString());
                throw new RpcException(new Status(StatusCode.Internal, "Error occurred while getting all comments."));
            }
        }


        public override async Task<CommentFilteredResponse> GetCommentsByUserId(GetCommentsByUserIdRequest request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.UserId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "User ID must be entered."));
            }
            if (!Guid.TryParse(request.UserId, out Guid userId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid user ID format"));
            }

            var comments = await _context.Comments
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            if (comments.Count == 0)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"No comments found for {userId}"));
            }

            var commentResponses = comments.Select(c => new CommentFiltered
            {
                Id = c.Id.ToString(),
                Body = c.Body,
                CreatedAt = Timestamp.FromDateTime(DateTime.SpecifyKind(c.CreatedAt, DateTimeKind.Utc)),
                Upvotes = c.Upvotes ?? 0,
                AuthorId = c.AuthorId.ToString(),
            });

            var response = new CommentFilteredResponse
            {
                Response = { commentResponses }
            };

            return response;
        }

        public override async Task<CommentFilteredResponse> GetCommentsByISBN(GetCommentsByISBNRequest request, ServerCallContext context)
        {
            if (string.IsNullOrEmpty(request.Isbn))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "ISBN is required."));
            }

            Regex regex = new Regex(@"^\d{10}|\d{13}$");
            if (!regex.IsMatch(request.Isbn))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "ISBN must be 10 or 13 digits."));
            }

            var comments = await _context.Comments
                .Where(c => c.ISBN10 == request.Isbn || c.ISBN13 == request.Isbn)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            if (comments.Count == 0)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "No comments for this book yet."));
            }

            var commentResponses = comments.Select(c => new CommentFiltered
            {
                Id = c.Id.ToString(),
                Body = c.Body,
                CreatedAt = Timestamp.FromDateTime(DateTime.SpecifyKind(c.CreatedAt, DateTimeKind.Utc)),
                Upvotes = c.Upvotes ?? 0,
                AuthorId = c.AuthorId.ToString(),
            });

            var response = new CommentFilteredResponse
            {
                Response = { commentResponses }
            };

            return response;
        }


        public override async Task<CommentListedResponse> GetCommentsByAuthorId(GetCommentsByAuthorIdRequest request, ServerCallContext context)
        {
            _logger.LogInformation("GetCommentsByAuthorId is called");
            if (string.IsNullOrWhiteSpace(request.AuthorId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Author ID must be provided."));
            }
            if (!Guid.TryParse(request.AuthorId, out Guid authorId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid author ID format"));
            }
            

            if (!_context.Comments.Any(c => c.AuthorId == (authorId)))
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"no comments found for {request.AuthorId}"));
            }

            var comments = await _context.Comments
                .Where(c => c.AuthorId == authorId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();


            var commentResponses = comments.Select(c => new CommentTwo
            {
                Id = c.Id.ToString(),
                Body = c.Body,
                CreatedAt = Timestamp.FromDateTime(DateTime.SpecifyKind(c.CreatedAt, DateTimeKind.Utc)),
                Upvotes = c.Upvotes ?? 0,

                ParentCommentId = c.ParentCommentId?.ToString() ?? "",
                CommentType = (CommentType)c.CommentType,
                ISBN10 = c.ISBN10 ?? "",
                ISBN13 = c.ISBN13 ?? "",
                UserId = c.UserId?.ToString() ?? ""

            });
            var response = new CommentListedResponse
            {
                Response = { commentResponses }
            };

            return response;
        }



        public override async Task<MessageResponse> DeleteComment(DeleteCommentRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.Id, out Guid commentId))
            {
                _logger.LogWarning("Invalid comment id format. Use Guid");
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid comment id format"));
            }

            var commentEntity = await _context.Comments.FindAsync(commentId);

            if (commentEntity == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Comment not found"));
            }

            
            var children = await _context.Comments.Where(c => c.ParentCommentId == commentId).ToListAsync();
            if (children.Any())
            {
                
                foreach (var child in children)
                {
                    await DeleteComment(new DeleteCommentRequest { Id = child.Id.ToString() }, context);
                }
            }

            _context.Comments.Remove(commentEntity);
            await _context.SaveChangesAsync();

            var response = new MessageResponse { Message = $"{request.Id} successfully deleted!"   };
            return response;
        }





        public override async Task<MessageResponse> CreateComment(CreateCommentRequest request, ServerCallContext context)
        {
            
            if (string.IsNullOrWhiteSpace(request.Body))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "The comment body is required."));
            }

            if (request.AuthorId == null || request.UserId == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "The author ID and user ID are required."));
            }
            var comment = new V1Comments
            {
                Id = Guid.NewGuid(),
                Body = request.Body,
                CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow).ToDateTime(),
                Upvotes = request.Upvotes,
                AuthorId = Guid.Parse(request.AuthorId),
                CommentType = ClassLibrary.Enums.V1.CommentType.User,
                UserId = Guid.Parse(request.UserId)
            };

            try
            {
               
                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Comment with ID {CommentId} added successfully", comment.Id);
                _logger.LogInformation($"Comment with ID {comment.Body} added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment with ID {CommentId}", comment.Id);
                throw new RpcException(new Status(StatusCode.Internal, "An error occurred while adding the comment."));
            }

            var commentResponse = new MessageResponse
            {
                Message = $"{comment.Id} added successfully!!"
            };

            return commentResponse;
        }


        public override async Task<MessageResponse> CreateBookComment(CreateBookCommentRequest request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.Body))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "The comment body is required."));
            }
            /*
            if (string.IsNullOrEmpty(request.ISBN10) && string.IsNullOrEmpty(request.ISBN13))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Either ISBN10 or ISBN13 must be provided."));
            }
            */
            if (!string.IsNullOrEmpty(request.ISBN10) && !Regex.IsMatch(request.ISBN10, @"^\d{10}$"))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "ISBN must be 10 or 13 digits"));
            }
            if (!string.IsNullOrEmpty(request.ISBN13) && !Regex.IsMatch(request.ISBN13, @"^\d{13}$"))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "ISBN must be 10 or 13 digits"));
            }
            if (request.AuthorId == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "The author ID is required."));
            }
            var comment = new V1Comments
            {
                Id = Guid.NewGuid(),
                Body = request.Body,
                CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow).ToDateTime(),
                Upvotes = request.Upvotes,
                AuthorId = Guid.Parse(request.AuthorId),
                CommentType = ClassLibrary.Enums.V1.CommentType.Book,
                ISBN10 = request.ISBN10,
                ISBN13 = request.ISBN13,
            };

            try
            {
                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Comment with ID {CommentId} added successfully", comment.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment with ID {CommentId}", comment.Id);
                throw;
            }

            var commentResponse = new MessageResponse { Message = $"{comment.Id} added successfully!!" };
            return commentResponse;
        }


        public override async Task<MessageResponse> CreateReplyComment(CreateReplyCommentRequest request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.Body))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "The comment body is required."));
            }
            if (request.AuthorId == null || request.ParentCommentId == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "The author ID and parent comment ID are required."));
            }
            var comment = new V1Comments
            {
                Id = Guid.NewGuid(),
                Body = request.Body,
                CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow).ToDateTime(),
                Upvotes = request.Upvotes,
                AuthorId = Guid.Parse(request.AuthorId),
                CommentType = ClassLibrary.Enums.V1.CommentType.Reply,
                ParentCommentId = Guid.Parse(request.ParentCommentId)
            };

            try
            {
                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Comment with ID {CommentId} added successfully", comment.Id);
                _logger.LogInformation($"{comment}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment with ID {CommentId}", comment.Id);
                Debug.WriteLine(comment);

                throw;
            }

            var commentResponse = new MessageResponse { Message = $"{comment.Id} added successfully!!" };

            return commentResponse;
        }



        public override async Task<MessageResponse> UpdateComment(UpdateCommentRequest request, ServerCallContext context)
        {

            if (!Guid.TryParseExact(request.Id, "D", out Guid commentId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid comment id format"));
            }
            if (string.IsNullOrWhiteSpace(request.Body))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "The comment body is required."));
            }

            var comment = await _context.Comments.FindAsync(commentId);


            if (comment == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Comment not found"));
            }


            comment.Body = request.Body;
            comment.CreatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();


            var response = new MessageResponse
            {
                Message = $"Successfully updated! Updated comment is: {comment.Body}" ,
                
            };

            return response;
        }

        public override async Task<MessageResponse> IncrementUpvotes(IncrementUpvotesRequest request, ServerCallContext context)
        {
           
            if (!Guid.TryParse(request.Id, out Guid commentId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid comment id format"));
            }

            
            var comment = await _context.Comments.FindAsync(commentId);

           
            if (comment == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Comment not found"));
            }

            
            comment.Upvotes ++;
            await _context.SaveChangesAsync();

            
            var response = new MessageResponse { Message = $"Upvotes successfully incremented! Total upvotes is: {comment.Upvotes}." };

            return response;
        }

     


    }




}

