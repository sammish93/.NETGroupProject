using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MarketplaceModels;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Data;

namespace Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Services
{
    public class V1MarketplaceService : V1IMarketplace
    {
        private readonly MarketplaceContext _context;

        public V1MarketplaceService(MarketplaceContext context)
        {
            _context = context;
        }

        public async Task CreateNewPost(V1MarketplaceBook post)
        {
            var newPost = new V1MarketplaceBook
            {
                Id = Guid.NewGuid(),
                Condition = post.Condition,
                Price = post.Price,
                Currency = post.Currency,
                Status = post.Status,
                OwnerId = post.OwnerId,
                DateCreated = new DateTime(),
                DateModified = new DateTime()
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
        }

        public Task<bool> DeletePost(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<V1MarketplaceBook> GetAllPosts()
        {
            throw new NotImplementedException();
        }

        public Task<V1MarketplaceBook> GetPostById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePost(Guid id, V1MarketplaceBook post)
        {
            throw new NotImplementedException();
        }
    }
}

