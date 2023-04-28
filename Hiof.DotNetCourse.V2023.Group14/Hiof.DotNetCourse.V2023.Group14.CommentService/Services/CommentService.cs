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
                var commentEntity = _context.Comments.Include(c => c.Replies).FirstOrDefault(c => c.Id == Guid.Parse(request.Id));

                if (commentEntity == null)
                {
                    _logger.LogWarning("{StatusCode}Comment with ID {Id} not found", request.Id);
                    
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while handling GetComment request");
                throw new RpcException(new Status(StatusCode.Internal, "An error occurred while handling the request"), ex.Message);
            }
        }



    

        public override async Task<CommentList> GetAllComments(Empty request, ServerCallContext context)
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





        public override async Task<DeleteCommentResponse> DeleteComment(DeleteCommentRequest request, ServerCallContext context)
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

            var response = new DeleteCommentResponse { Id = commentEntity.Id.ToString() };
            return response;
        }





        public override async Task<GetCommentRequest> CreateComment(CreateCommentRequest request, ServerCallContext context)
        {
            var comment = new V1Comments
            {
                Id = Guid.NewGuid(),
                Body = request.Body,
                CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow).ToDateTime(),
                Upvotes = request.Upvotes,
                AuthorId = Guid.Parse(request.AuthorId),
                CommentType = (ClassLibrary.Enums.V1.CommentType)request.CommentType,
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
                Debug.WriteLine(comment);
                
                throw;
            }

            var commentResponse = new GetCommentRequest
            {
                Id = comment.Id.ToString(),
            };

            return commentResponse;
        }

        public override async Task<GetCommentRequest> CreateBookComment(CreateBookCommentRequest request, ServerCallContext context)
        {
            var comment = new V1Comments
            {
                Id = Guid.NewGuid(),
                Body = request.Body,
                CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow).ToDateTime(),
                Upvotes = request.Upvotes,
                AuthorId = Guid.Parse(request.AuthorId),
                CommentType = ClassLibrary.Enums.V1.CommentType.Book,
                ISBN10 = request.ISBN10,
                ISBN13= request.ISBN13,
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
                Debug.WriteLine(comment);

                throw;
            }

            var commentResponse = new GetCommentRequest
            {
                Id = comment.Id.ToString(),
            };

            return commentResponse;
        }
        public override async Task<GetCommentRequest> CreateReplyComment(CreateReplyCommentRequest request, ServerCallContext context)
        {
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
                _logger.LogInformation("Comment with ID {CommentId} added successfully", comment.CommentType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment with ID {CommentId}", comment.Id);
                Debug.WriteLine(comment);

                throw;
            }

            var commentResponse = new GetCommentRequest
            {
                Id = comment.Id.ToString(),
            };

            return commentResponse;
        }



        public override async Task<UpdateCommentResponse> UpdateComment(UpdateCommentRequest request, ServerCallContext context)
        {

            if (!Guid.TryParseExact(request.Id, "D", out Guid commentId))
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

        public override async Task<IncrementUpvotesResponse> IncrementUpvotes(IncrementUpvotesRequest request, ServerCallContext context)
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

            
            var response = new IncrementUpvotesResponse
            {
                Id = comment.Id.ToString(),
                Upvotes = (int)comment.Upvotes 
            };

            return response;
        }

        public override async Task<CommentFilteredResponse> GetCommentsByUserId(GetCommentsByUserIdRequest request, ServerCallContext context)
        {
           
            var comments = await _context.Comments
                .Where(c => c.UserId == Guid.Parse(request.UserId))
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

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
            var comments = await _context.Comments
                .Where(c => c.ISBN10 == request.Isbn || c.ISBN13 == request.Isbn)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();


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
            var authorId = Guid.Parse(request.AuthorId);

            if (!_context.Comments.Any(c => c.AuthorId == authorId))
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Comments with AuthorId '{request.AuthorId}' not found."));
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


    }




}

