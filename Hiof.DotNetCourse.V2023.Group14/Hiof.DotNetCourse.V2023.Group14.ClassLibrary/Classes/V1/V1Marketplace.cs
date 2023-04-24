using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1
{
	/// <summary>
	/// Model class for the marketplace service.
	/// </summary>
	public class V1Marketplace
	{

		public Guid Id { get; set; }
		public string Condition { get; set; }
		public decimal Price { get; set; }
		public V1Currency Currency { get; set; }
		public V1BookStatus Status { get; set; }
		public DateTime dateCreated { get; set; }
		public DateTime dateModified { get; set; }

	}
}

