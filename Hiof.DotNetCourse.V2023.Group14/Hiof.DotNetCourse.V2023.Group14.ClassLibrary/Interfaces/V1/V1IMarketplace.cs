using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1
{
	/// <summary>
	/// Contract for the V1Marketplace controller.
	/// </summary>
	public interface V1IMarketplace
	{
		Task<V1Marketplace> GetAllPosts();
		Task<V1Marketplace> GetPostById(Guid id);
		Task CreateNewPost(V1Marketplace post);
		Task UpdatePost(Guid id, V1Marketplace post);
		Task<bool> DeletePost(Guid id);
	}
}

