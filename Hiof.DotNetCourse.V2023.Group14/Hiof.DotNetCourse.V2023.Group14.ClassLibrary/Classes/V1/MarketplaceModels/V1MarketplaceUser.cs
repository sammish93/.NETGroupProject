using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MarketplaceModels
{
	/// <summary>
	/// Model class representing a user in the marketplace and his/her´s posts.
	/// </summary>
	[Table("marketplace_posts", Schema="dbo")]
	public class V1MarketplaceUser
	{
		[Key]
		[Column("OwnerId", TypeName= "char(36)")]
		public Guid OwnerId { get; set; }

		public List<V1MarketplaceBook> Posts { get; set; }
	}
}

