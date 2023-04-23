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
            var commentEntity = _context.Comments.FirstOrDefault(c => c.Id == Guid.Parse(request.Id));

            if (commentEntity == null)
            {
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

            return Task.FromResult(response);
        }

        public override async Task GetAllComments(Empty request, IServerStreamWriter<Comment> responseStream, ServerCallContext context)
        {
            var comments = await _context.Comments.ToListAsync();

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

                await responseStream.WriteAsync(comment);
            }
        }

        public override async Task<CommentResponse> UpdateComment(UpdateCommentRequest request, ServerCallContext context)
        {
            var comment = await _context.Comments.FindAsync(Guid.Parse(request.Id));

            if (comment == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Comment not found"));
            }

            comment.Body = request.Body;

            await _context.SaveChangesAsync();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Comment, CommentResponse>()
                    .ForMember(dest => dest.Comment,
                        opt => opt.MapFrom(src => src));
            });

            var mapper = new Mapper(config);

            var commentResponse = mapper.Map<CommentResponse>(comment);

            return commentResponse;
        }

        public override async Task<DeleteCommentResponse> DeleteComment(DeleteCommentRequest request, ServerCallContext context)
        {
            var commentId = await _context.Comments.FindAsync(Guid.Parse(request.Id));
            // perform deletion logic
            return new DeleteCommentResponse
            {
                Success = true
            };
        }



            /*
            public override Task<Comment> GetParentComment(CommentRequest request, ServerCallContext context)
            {
                var commentEntity = _context.Comments.Include(c => c.Replies).FirstOrDefault(c => c.Id == Guid.Parse(request.Id));

                if (commentEntity == null)
                {
                    throw new RpcException(new Status(StatusCode.NotFound, $"Comment with ID {request.Id} not found"));
                }

                var comment = new Comment
                {
                    Id = commentEntity.Id.ToString(),

                    Body = commentEntity.Body,
                    CreatedAt = Timestamp.FromDateTime(DateTime.SpecifyKind(commentEntity.CreatedAt, DateTimeKind.Utc)),
                    Upvotes = commentEntity.Upvotes ?? 0,
                    AuthorId = commentEntity.AuthorId.ToString(),
                    CommentType = (CommentType)commentEntity.CommentType,
                    ISBN10 = commentEntity.ISBN10,
                    ISBN13 = commentEntity.ISBN13,
                    UserId = commentEntity.UserId?.ToString(),
                };

                var replies = new List<CommentReply>();

                foreach (var replyEntity in commentEntity.Replies)
                {
                    var reply = new CommentReply
                    {
                        Id = replyEntity.Id.ToString(),
                        Body = replyEntity.Body,
                        CreatedAt = Timestamp.FromDateTime(DateTime.SpecifyKind(replyEntity.CreatedAt, DateTimeKind.Utc)),
                        Upvotes = replyEntity.Upvotes ?? 0,
                    };

                    replies.Add(reply);
                }

                comment.Replies.AddRange(replies);

                return Task.FromResult(comment);
            }
            public override async Task<CreateCommentReplyResponse> CreateCommentReply(CommentReply request, ServerCallContext context)
            {
                var existingComment = await _context.V1CommentReplies.FirstOrDefaultAsync(c => c.Id == Guid.Parse(request.Id));
                if (existingComment != null)
                {
                    throw new RpcException(new Status(StatusCode.AlreadyExists, $"Comment reply with ID '{request.Id}' already exists."));
                }

                var commentReply = new V1ParentComment
                {
                    Id = Guid.NewGuid(),
                    Body = request.Body,
                    CreatedAt = request.CreatedAt.ToDateTime(),
                    AuthorId = Guid.Parse(request.AuthorId),
                    ParentCommentId = Guid.Parse(request.ParentCommentId),
                    Upvotes = request.Upvotes
                };

                await _context.V1CommentReplies.AddAsync(commentReply);
                await _context.SaveChangesAsync();

                return new CreateCommentReplyResponse { CommentReplyId = commentReply.Id.ToString() };
            }



            */



        }




}

