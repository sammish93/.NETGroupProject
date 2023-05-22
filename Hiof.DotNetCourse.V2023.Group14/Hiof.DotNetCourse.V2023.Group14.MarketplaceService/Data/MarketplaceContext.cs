using System;
using System.Text;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MarketplaceModels;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Microsoft.EntityFrameworkCore;

namespace Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Data
{
	public class MarketplaceContext : DbContext
	{
		public MarketplaceContext(DbContextOptions<MarketplaceContext> options): base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<V1MarketplaceBook>()
				.HasOne<V1MarketplaceUser>()
				.WithMany(user => user.Posts)
				.HasForeignKey(book => book.OwnerId)
				.HasConstraintName("FK_marketplace_owner")
				.OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<V1MarketplaceUser>().HasData(
                new
                {
                   OwnerId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6")
				},
                new
                {
                    OwnerId = Guid.Parse("54af86bf-346a-4cba-b36f-527748e1cb93")
                });

            modelBuilder.Entity<V1MarketplaceBook>().HasData(
                new
                {
                    Id = Guid.Parse("6b859b12-3001-43b5-bb4a-58155f07d63a"),
					Condition = "used",
					Price = Decimal.Parse("8.0"),
					Currency = V1Currency.USD,
					Status = V1BookStatus.UNSOLD,
					OwnerId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
					DateCreated = DateTime.Parse("2023-05-17 08:32:42.407"),
					DateModified = DateTime.Parse("2023-05-17 08:32:42.407"),
					ISBN10 = "1410350339",
					ISBN13 = "9781410350336"
                },
                new
                {
                    Id = Guid.Parse("af728d4e-2879-4cda-bc6b-68101a65e191"),
                    Condition = "boka er helt nytt",
                    Price = Decimal.Parse("299.00"),
                    Currency = V1Currency.NOK,
                    Status = V1BookStatus.UNSOLD,
                    OwnerId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                    DateCreated = DateTime.Parse("2023-05-17 08:22:20.457"),
                    DateModified = DateTime.Parse("2023-05-17 08:22:20.457"),
                    ISBN10 = "8771290060",
                    ISBN13 = "9788771290066"
                },
                new
                {
                    Id = Guid.Parse("2c775125-f0a8-46e0-bc3a-837c07414a56"),
                    Condition = "Bit of wear around the edges",
                    Price = Decimal.Parse("20.0"),
                    Currency = V1Currency.USD,
                    Status = V1BookStatus.UNSOLD,
                    OwnerId = Guid.Parse("54af86bf-346a-4cba-b36f-527748e1cb93"),
                    DateCreated = DateTime.Parse("2023-04-17 08:22:20.457"),
                    DateModified = DateTime.Parse("2023-04-17 08:22:20.457"),
                    ISBN10 = "8771290060",
                    ISBN13 = "9788771290066"
                },
                new
                {
                    Id = Guid.Parse("cf6cb29f-0f29-44af-a0f0-a49cde7553ca"),
                    Condition = "new",
                    Price = Decimal.Parse("200.0"),
                    Currency = V1Currency.NOK,
                    Status = V1BookStatus.UNSOLD,
                    OwnerId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                    DateCreated = DateTime.Parse("2023-05-16 13:15:01.870"),
                    DateModified = DateTime.Parse("2023-05-16 13:15:01.870"),
                    ISBN10 = "0545919665",
                    ISBN13 = "9780545919661"
                },
                new
                {
                    Id = Guid.Parse("cff03515-102b-4831-a599-fcde49b9b344"),
                    Condition = "new (packaged)",
                    Price = Decimal.Parse("25.00"),
                    Currency = V1Currency.EUR,
                    Status = V1BookStatus.UNSOLD,
                    OwnerId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                    DateCreated = DateTime.Parse("2023-05-17 12:36:59.333"),
                    DateModified = DateTime.Parse("2023-05-17 12:36:59.333"),
                    ISBN10 = "2808018630",
                    ISBN13 = "9782808018630"
                });
        }

		public DbSet<V1MarketplaceUser> MarketplaceUser { get; set; }

		public DbSet<V1MarketplaceBook> MarketplaceBooks { get; set; }
	}
}

