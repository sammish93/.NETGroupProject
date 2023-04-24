using System;
namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MarketplaceModels
{
	public class V1MarketplacePosts
	{
		public Guid OwnerId { get; set; }

		public List<V1MarketplaceBook> Books { get; set; }
	}
}

