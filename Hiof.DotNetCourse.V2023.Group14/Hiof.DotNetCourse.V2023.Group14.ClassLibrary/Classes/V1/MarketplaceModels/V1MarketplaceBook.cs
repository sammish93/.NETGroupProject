using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
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
		[JsonIgnore]
		public Guid Id { get; set; }

		[Column("Condition", TypeName = "varchar(255)")]
		public string Condition { get; set; }

		[Column("Price", TypeName = "decimal(10, 2)")]
		public decimal Price { get; set; }

		[Column("Currency", TypeName = "varchar(50)")]
		[EnumDataType(typeof(V1Currency))]
		[JsonIgnore]
        public V1Currency Currency { get; set; }

		[Column("Status", TypeName = "varchar(50)")]
		[EnumDataType(typeof(V1BookStatus))]
		[JsonIgnore]
        public V1BookStatus Status { get; set; }

		[ForeignKey("OwnerId")]
		[Column("OwnerId", TypeName = "char(36)")]
		[JsonIgnore]
		public Guid OwnerId { get; set; }

		[Column("DateCreated", TypeName = "datetime")]
		[JsonIgnore]
		public DateTime DateCreated { get; set; }

		[Column("DateModified", TypeName = "datetime")]
		[JsonIgnore]
		public DateTime DateModified { get; set; }

	}
}

