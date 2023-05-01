﻿// <auto-generated />
using System;
using Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Migrations
{
    [DbContext(typeof(MarketplaceContext))]
    partial class MarketplaceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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