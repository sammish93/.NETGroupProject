using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MarketplaceModels;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;

namespace Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Services
{
    public class V1MarketplaceService : V1IMarketplace
    {
        public Task CreateNewPost(V1MarketplaceBook post)
        {
            throw new NotImplementedException();
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

