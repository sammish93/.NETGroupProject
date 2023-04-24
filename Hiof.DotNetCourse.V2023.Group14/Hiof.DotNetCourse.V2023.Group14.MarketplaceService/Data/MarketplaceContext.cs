using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Microsoft.EntityFrameworkCore;

namespace Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Data
{
	public class MarketplaceContext : DbContext
	{
		public MarketplaceContext(DbContextOptions<MarketplaceContext> options): base(options)
		{
		}

		public DbSet<V1MarketplacePost> MarketplacePost { get; set; }

		public DbSet<V1MarketplaceBook> MarketplaceBooks { get; set; }
	}
}

