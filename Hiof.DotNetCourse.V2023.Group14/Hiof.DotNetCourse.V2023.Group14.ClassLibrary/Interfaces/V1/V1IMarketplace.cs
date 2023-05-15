using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MarketplaceModels;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1
{
	/// <summary>
	/// Contract for the V1Marketplace controller.
	/// </summary>
	public interface V1IMarketplace
	{
		Task<List<V1MarketplaceBookResponse>> GetAllPosts();

		Task<V1MarketplaceBookResponse?> GetPostById(Guid id);

		Task<V1MarketplaceBookResponse> GetPostByIsbn(string isbn);

		Task<bool> CreateNewPost(Guid ownerId, V1Currency currency, V1BookStatus status, V1MarketplaceBook post);

		Task<string> UpdatePost(Guid postId, V1MarketplaceBookUpdated post);

		Task<bool> DeletePost(Guid id);
	}
}

