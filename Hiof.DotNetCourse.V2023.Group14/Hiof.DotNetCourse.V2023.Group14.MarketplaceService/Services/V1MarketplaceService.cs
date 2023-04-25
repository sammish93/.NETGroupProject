using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MarketplaceModels;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Data;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Microsoft.EntityFrameworkCore;

namespace Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Services
{
    public class V1MarketplaceService : V1IMarketplace
    {
        private readonly MarketplaceContext _context;

        public V1MarketplaceService(MarketplaceContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateNewPost(Guid ownerId, V1Currency currency, V1BookStatus status, V1MarketplaceBook post)
        {
            var newPost = new V1MarketplaceBook
            {
                Id = Guid.NewGuid(),
                Condition = post.Condition,
                Price = post.Price,
                Currency = currency,
                Status = status,
                OwnerId = ownerId,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow
            };

            // Find user with the given owner id
            var user = await _context.MarketplaceUser.FindAsync(post.OwnerId);

            if (user == null)
            {
                // Create new marketplace user
                user = new V1MarketplaceUser
                {
                    OwnerId = post.OwnerId,
                    Posts = new List<V1MarketplaceBook>()
                };
                // Add the new user to database. 
                await _context.MarketplaceUser.AddAsync(user);
            }

            // Add the new post to the users Posts list.
            user.Posts.Add(newPost);
            await _context.MarketplaceBooks.AddAsync(newPost);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePost(Guid postId)
        {
            var post = await _context.MarketplaceBooks.FindAsync(postId);
            if (post != null)
            {
                _context.Remove(post);
                var rowsAffected = await _context.SaveChangesAsync();
                if (rowsAffected == 0)
                {
                    throw new Exception("Failed to delete post!");
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<V1MarketplaceBookResponse>> GetAllPosts()
        {
            var postList = await _context.MarketplaceBooks.ToListAsync();
            return postList.Select(post => new V1MarketplaceBookResponse
            {
                Id = post.Id,
                Condition = post.Condition,
                Price = post.Price,
                Currency = post.Currency,
                Status = post.Status,
                OwnerId = post.OwnerId,
                DateCreated = post.DateCreated,
                DateModified = post.DateModified,

            }).ToList();
        }

        public async Task<V1MarketplaceBookResponse?> GetPostById(Guid id)
        {
            var post = await _context.MarketplaceBooks.FindAsync(id);
            if (post != null)
            {
                return new V1MarketplaceBookResponse
                {
                    Id = post.Id,
                    Condition = post.Condition,
                    Price = post.Price,
                    Currency = post.Currency,
                    Status = post.Status,
                    OwnerId = post.OwnerId,
                    DateCreated = post.DateCreated,
                    DateModified = post.DateModified
                };
            }
            return null;
        }

        public async Task<bool> UpdatePost(V1MarketplaceBookResponse post)
        {
            var existingPost = await _context.MarketplaceBooks.FindAsync(post.Id);
            if (existingPost == null)
            {
                return false;
            }
            else
            {
                existingPost.Id = post.Id;
                existingPost.Condition = post.Condition;
                existingPost.Price = post.Price;
                existingPost.Currency = post.Currency;
                existingPost.Status = post.Status;
                existingPost.OwnerId = post.OwnerId;
                existingPost.DateModified = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return true;
            }
        }
    }
}

