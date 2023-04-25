using System;
using System.Text.Json.Serialization;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MarketplaceModels
{
    /// <summary>
    /// Model used to update an post in the marketplace.
    /// </summary>
	public class V1MarketplaceBookUpdated
	{
        [JsonIgnore]
        public Guid Id { get; set; }
        public string Condition { get; set; }
        public decimal Price { get; set; }
        public V1Currency Currency { get; set; }
        public V1BookStatus Status { get; set; }
        public Guid OwnerId { get; set; }
        [JsonIgnore]
        public DateTime DateCreated { get; set; }
        [JsonIgnore]
        public DateTime DateModified { get; set; }

    }
}

