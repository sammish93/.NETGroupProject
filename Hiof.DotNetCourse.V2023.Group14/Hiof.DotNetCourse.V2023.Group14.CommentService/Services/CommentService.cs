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
            _logger.LogInformation("GetComment method called with Id {Id}", request.Id);

            var commentEntity = _context.Comments.Include(c => c.Replies).FirstOrDefault(c => c.Id == Guid.Parse(request.Id));

            if (commentEntity == null)
            {
                _logger.LogWarning("Comment with ID {Id} not found", request.Id);
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




        public override async Task<CommentList> GetAllComments(Empty request, ServerCallContext context)
        {
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


        public override async Task<UpdateCommentResponse> UpdateComment(UpdateCommentRequest request, ServerCallContext context)
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

            
            comment.Body = request.Body;
            comment.CreatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            
            var response = new UpdateCommentResponse
            {
                Id = comment.Id.ToString(),
                Body = comment.Body,
                CreatedAt = Timestamp.FromDateTime(DateTime.SpecifyKind(comment.CreatedAt, DateTimeKind.Utc))
            };

            return response;
        }


        public override async Task<DeleteCommentResponse> DeleteComment(DeleteCommentRequest request, ServerCallContext context)
        {
            // Check if request.Id is a valid GUID format
            if (!Guid.TryParse(request.Id, out Guid commentId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid comment id format"));
            }

            // Retrieve the comment from the DbContext
            var commentEntity = await _context.Comments.FindAsync(commentId);

            // If the comment doesn't exist, throw an exception
            if (commentEntity == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Comment not found"));
            }

            // Remove the comment from the DbContext and save changes
            _context.Comments.Remove(commentEntity);
            await _context.SaveChangesAsync();

            // Return the id of the deleted comment
            var response = new DeleteCommentResponse { Id = commentEntity.Id.ToString() };
            return response;
        }

        public override async Task<GetCommentRequest> CreateComment(CreateCommentRequest request, ServerCallContext context)
        {
            // Check if request.Id is a valid GUID format
            if (!Guid.TryParse(request.Id, out Guid commentId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid comment id format"));
            }

            // Set created_at field to current timestamp
            request.CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow);

            // Create a new CommentEntity object and map fields from Comment message
            var commentEntity = new V1Comments
            {
                Id = commentId,
                Body = request.Body,
                CreatedAt = request.CreatedAt.ToDateTime(),
                Upvotes = request.Upvotes,
                AuthorId = Guid.Parse(request.AuthorId),
                CommentType = (ClassLibrary.Enums.V1.CommentType)request.CommentType,
                ISBN10 = request.ISBN10,
                ISBN13 = request.ISBN13,
                
            };

            // Set ParentCommentId property to null if request.ParentCommentId is null
            // Set ParentCommentId property to null if request.ParentCommentId is null or CommentType is not Reply
            if (request.ParentCommentId == null || request.CommentType != CommentType.Reply)
            {
                commentEntity.ParentCommentId = null;
            }
            else
            {
                // Check if request.ParentCommentId is a valid GUID format
                if (!Guid.TryParse(request.ParentCommentId, out Guid parentCommentId))
                {
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid parent comment id format"));
                }
                commentEntity.ParentCommentId = parentCommentId;
            }
            if (request.UserId == null || request.CommentType != CommentType.User)
            {
                commentEntity.UserId = null;
            }
            else
            {
                // Check if request.ParentCommentId is a valid GUID format
                if (!Guid.TryParse(request.UserId, out Guid UserId))
                {
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid parent comment id format"));
                }
                commentEntity.UserId = UserId;
            }

            // Add the comment entity to the DbContext and save changes
            _context.Comments.Add(commentEntity);
            await _context.SaveChangesAsync();

            // Return the GetCommentRequest with the id of the newly created comment
            var response = new GetCommentRequest { Id = commentEntity.Id.ToString() };
            return response;
        }

        public override async Task<IncrementUpvotesResponse> IncrementUpvotes(IncrementUpvotesRequest request, ServerCallContext context)
        {
            // Check if request.Id is a valid GUID format
            if (!Guid.TryParse(request.Id, out Guid commentId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid comment id format"));
            }

            // Get the comment from the database
            var comment = await _context.Comments.FindAsync(commentId);

            // Throw an exception if the comment doesn't exist
            if (comment == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Comment not found"));
            }

            // Increment the upvotes and save changes
            comment.Upvotes++;
            await _context.SaveChangesAsync();

            // Return the updated comment ID and upvotes count
            var response = new IncrementUpvotesResponse
            {
                Id = comment.Id.ToString(),
                Upvotes = comment.Upvotes ?? 0
            };

            return response;
        }



    }




}

