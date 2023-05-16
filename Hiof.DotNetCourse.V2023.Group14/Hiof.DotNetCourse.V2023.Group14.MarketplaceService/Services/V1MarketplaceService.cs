using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MarketplaceModels;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Data;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Services
{
    public class V1MarketplaceService : V1IMarketplace
    {
        private readonly MarketplaceContext _context;

        public V1MarketplaceService(MarketplaceContext context)
        {
            _context = context;
        }

        public async Task<IList<V1MarketplaceBookResponse>?> GetPostByIsbn(string isbn)
        {
            var postList = await (from posts in _context.MarketplaceBooks where posts.ISBN10 == isbn || posts.ISBN13 == isbn
                   select posts).ToListAsync();


            if (!postList.IsNullOrEmpty())
            {
                IList<V1MarketplaceBookResponse> postListConverted = new List<V1MarketplaceBookResponse>();

                foreach (V1MarketplaceBook post in postList)
                {
                    postListConverted.Add(new V1MarketplaceBookResponse
                    {
                        Id = post.Id,
                        Condition = post.Condition,
                        Price = post.Price,
                        Currency = post.Currency,
                        Status = post.Status,
                        OwnerId = post.OwnerId,
                        DateCreated = post.DateCreated,
                        DateModified = post.DateModified,
                        ISBN10 = post.ISBN10 ?? "",
                        ISBN13 = post.ISBN13 ?? ""
                    });  
                }

                return postListConverted;
            }
            return null;

        }

        public async Task<bool> CreateNewPost(Guid ownerId, V1Currency currency, V1BookStatus status, V1MarketplaceBook post)
        {
            var isbn10 = post.ISBN10;
            var isbn13 = post.ISBN13;

            if (isbn10 != null && isbn10.Equals("string"))
                isbn10 = null;

            else if (isbn13 != null && isbn13.Equals("string"))
                isbn13 = null;

            var newPost = new V1MarketplaceBook
            {
                Id = Guid.NewGuid(),
                Condition = post.Condition,
                Price = post.Price,
                Currency = currency,
                Status = status,
                OwnerId = ownerId,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
                ISBN10 = isbn10,
                ISBN13 = isbn13
            };

            // Find user with the given owner id
            var user = await _context.MarketplaceUser
                .Include(u => u.Posts)
                .FirstOrDefaultAsync(u => u.OwnerId == ownerId);

            if (user == null)
            {
                // Create new marketplace user
                user = new V1MarketplaceUser
                {
                    OwnerId = ownerId,
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
                   DateModified = post.DateModified,
                   ISBN10 = post.ISBN10 ?? "",
                   ISBN13 = post.ISBN13 ?? ""
                };
            }
            return null;
        }

        public async Task<string> UpdatePost(Guid postId, V1MarketplaceBookUpdated post)
        {
            var existingPost = await _context.MarketplaceBooks.FindAsync(postId);
            if (existingPost == null)
            {
                return "The id does not exists, please provide a valid id.";
            }
            else if (existingPost.OwnerId != post.OwnerId)
            {
                return "Wrong ownerId, please provide the right one.";
            }
            else
            {
                string? isbn10 = post.ISBN10;
                string? isbn13 = post.ISBN13;

                if (isbn10.Equals("string"))
                    isbn10 = null;

                if (isbn13.Equals("string"))
                    isbn13 = null;


                existingPost.Condition = post.Condition;
                existingPost.Price = post.Price;
                existingPost.Currency = post.Currency;
                existingPost.Status = post.Status;
                existingPost.OwnerId = post.OwnerId;
                existingPost.DateModified = DateTime.UtcNow;
                existingPost.ISBN10 = isbn10;
                existingPost.ISBN13 = isbn13;

                await _context.SaveChangesAsync();
                return "Successfully updated the post!";
            }
        }
    }
}

