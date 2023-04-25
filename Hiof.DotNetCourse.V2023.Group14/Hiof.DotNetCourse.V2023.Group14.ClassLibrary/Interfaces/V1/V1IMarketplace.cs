using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1
{
	/// <summary>
	/// Contract for the V1Marketplace controller.
	/// </summary>
	public interface V1IMarketplace
	{
		Task<V1MarketplaceBook> GetAllPosts();

		Task<V1MarketplaceBook> GetPostById(Guid id);

		Task<bool> CreateNewPost(V1MarketplaceBook post);

		Task UpdatePost(Guid id, V1MarketplaceBook post);

		Task<bool> DeletePost(Guid id);
	}
}

