using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1;

namespace Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Services
{
	public class V1MarketplaceService : V1IMarketplace
	{
        // TODO: Add a referance to database context.

		public V1MarketplaceService()
		{
		}

        public Task CreateNewPost(V1Marketplace post)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePost(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<V1Marketplace> GetAllPosts()
        {
            throw new NotImplementedException();
        }

        public Task<V1Marketplace> GetPostById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePost(Guid id, V1Marketplace post)
        {
            throw new NotImplementedException();
        }
    }
}

