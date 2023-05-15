using System.ComponentModel.DataAnnotations.Schema;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MarketplaceModels
{
    /// <summary>
    /// Class used to map V1MarketplaceBook.
    /// </summary>
	public class V1MarketplaceBookResponse
	{
        [Column("Id", TypeName = "char(36)")]
        public Guid Id { get; set; }
        public string Condition { get; set; }
        public decimal Price { get; set; }
        public V1Currency Currency { get; set; }
        public V1BookStatus Status { get; set; }
        [Column("OwnerId", TypeName = "char(36)")]
        public Guid OwnerId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}

