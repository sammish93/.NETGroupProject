﻿// <auto-generated />
using System;
using Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Migrations
{
    [DbContext(typeof(MarketplaceContext))]
    [Migration("20230522082347_AddSeeding")]
    partial class AddSeeding
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MarketplaceModels.V1MarketplaceUser", b =>
                {
                    b.Property<string>("OwnerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("OwnerId");

                    b.HasKey("OwnerId");

                    b.ToTable("marketplace_posts", "dbo");

                    b.HasData(
                        new
                        {
                            OwnerId = "3fa85f64-5717-4562-b3fc-2c963f66afa6"
                        },
                        new
                        {
                            OwnerId = "54af86bf-346a-4cba-b36f-527748e1cb93"
                        });
                });

            modelBuilder.Entity("Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.V1MarketplaceBook", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("Id");

                    b.Property<string>("Condition")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("Condition");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("Currency");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime")
                        .HasColumnName("DateCreated");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("datetime")
                        .HasColumnName("DateModified");

                    b.Property<string>("ISBN10")
                        .HasColumnType("varchar(10)")
                        .HasColumnName("ISBN10");

                    b.Property<string>("ISBN13")
                        .HasColumnType("varchar(13)")
                        .HasColumnName("ISBN13");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("char(36)")
                        .HasColumnName("OwnerId");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("Price");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("Status");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("marketplace_books", "dbo");

                    b.HasData(
                        new
                        {
                            Id = "6b859b12-3001-43b5-bb4a-58155f07d63a",
                            Condition = "used",
                            Currency = "USD",
                            DateCreated = new DateTime(2023, 5, 17, 8, 32, 42, 407, DateTimeKind.Unspecified),
                            DateModified = new DateTime(2023, 5, 17, 8, 32, 42, 407, DateTimeKind.Unspecified),
                            ISBN10 = "1410350339",
                            ISBN13 = "9781410350336",
                            OwnerId = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                            Price = 8.0m,
                            Status = "UNSOLD"
                        },
                        new
                        {
                            Id = "af728d4e-2879-4cda-bc6b-68101a65e191",
                            Condition = "boka er helt nytt",
                            Currency = "NOK",
                            DateCreated = new DateTime(2023, 5, 17, 8, 22, 20, 457, DateTimeKind.Unspecified),
                            DateModified = new DateTime(2023, 5, 17, 8, 22, 20, 457, DateTimeKind.Unspecified),
                            ISBN10 = "8771290060",
                            ISBN13 = "9788771290066",
                            OwnerId = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                            Price = 299.00m,
                            Status = "UNSOLD"
                        },
                        new
                        {
                            Id = "2c775125-f0a8-46e0-bc3a-837c07414a56",
                            Condition = "Bit of wear around the edges",
                            Currency = "USD",
                            DateCreated = new DateTime(2023, 4, 17, 8, 22, 20, 457, DateTimeKind.Unspecified),
                            DateModified = new DateTime(2023, 4, 17, 8, 22, 20, 457, DateTimeKind.Unspecified),
                            ISBN10 = "8771290060",
                            ISBN13 = "9788771290066",
                            OwnerId = "54af86bf-346a-4cba-b36f-527748e1cb93",
                            Price = 20.0m,
                            Status = "UNSOLD"
                        },
                        new
                        {
                            Id = "cf6cb29f-0f29-44af-a0f0-a49cde7553ca",
                            Condition = "new",
                            Currency = "NOK",
                            DateCreated = new DateTime(2023, 5, 16, 13, 15, 1, 870, DateTimeKind.Unspecified),
                            DateModified = new DateTime(2023, 5, 16, 13, 15, 1, 870, DateTimeKind.Unspecified),
                            ISBN10 = "0545919665",
                            ISBN13 = "9780545919661",
                            OwnerId = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                            Price = 200.0m,
                            Status = "UNSOLD"
                        },
                        new
                        {
                            Id = "cff03515-102b-4831-a599-fcde49b9b344",
                            Condition = "new (packaged)",
                            Currency = "EUR",
                            DateCreated = new DateTime(2023, 5, 17, 12, 36, 59, 333, DateTimeKind.Unspecified),
                            DateModified = new DateTime(2023, 5, 17, 12, 36, 59, 333, DateTimeKind.Unspecified),
                            ISBN10 = "2808018630",
                            ISBN13 = "9782808018630",
                            OwnerId = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                            Price = 25.00m,
                            Status = "UNSOLD"
                        });
                });

            modelBuilder.Entity("Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.V1MarketplaceBook", b =>
                {
                    b.HasOne("Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MarketplaceModels.V1MarketplaceUser", null)
                        .WithMany("Posts")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_marketplace_owner");
                });

            modelBuilder.Entity("Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MarketplaceModels.V1MarketplaceUser", b =>
                {
                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}