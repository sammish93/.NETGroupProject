using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MarketplaceModels
{
    /// <summary>
    /// Model used to update an post in the marketplace.
    /// </summary>
	public class V1MarketplaceBookUpdated
	{
        [Column("OwnerId", TypeName = "char(36)")]
        public Guid OwnerId { get; set; }
        [JsonIgnore]
        [Column("Id", TypeName = "char(36)")]
        public Guid Id { get; set; }
        public string Condition { get; set; }
        public decimal Price { get; set; }
        public V1Currency Currency { get; set; }
        public V1BookStatus Status { get; set; }
        [JsonIgnore]
        public DateTime DateCreated { get; set; }
        [JsonIgnore]
        public DateTime DateModified { get; set; }
        public string ISBN10 { get; set; }
        public string ISBN13 { get; set; }

    }
}

