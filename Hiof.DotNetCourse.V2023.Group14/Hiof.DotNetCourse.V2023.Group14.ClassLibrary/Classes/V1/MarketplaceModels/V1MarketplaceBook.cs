using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1
{
	/// <summary>
	/// Model class representing a book in the marketplace service.
	/// </summary>
	[Table("marketplace_books", Schema="dbo")]
	public class V1MarketplaceBook
	{

		[Key]
		[Column("Id", TypeName = "char(36)")]
		public Guid Id { get; set; }

		[Column("Condition", TypeName = "varchar(255)")]
		public string Condition { get; set; }

		[Column("Price", TypeName = "decimal(3, 2)")]
		public decimal Price { get; set; }

		[Column("Currency", TypeName = "int")]
		[EnumDataType(typeof(V1Currency))]
		public V1Currency Currency { get; set; }

		[Column("Status", TypeName = "int")]
		[EnumDataType(typeof(V1BookStatus))]
		public V1BookStatus Status { get; set; }

		[ForeignKey("OwnerId")]
		[Column("OwnerId", TypeName = "char(36)")]
		public Guid OwnerId { get; set; }

		[Column("DateCreated", TypeName = "datetime")]
		public DateTime DateCreated { get; set; }

		[Column("DateModified", TypeName = "datetime")]
		public DateTime DateModified { get; set; }

	}
}

