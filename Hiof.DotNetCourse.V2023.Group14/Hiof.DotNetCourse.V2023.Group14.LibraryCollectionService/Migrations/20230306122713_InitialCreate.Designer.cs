﻿// <auto-generated />
using System;
using Hiof.DotNetCourse.V2023.Group14.LibraryCollectionService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Hiof.DotNetCourse.V2023.Group14.LibraryCollectionService.Migrations
{
    [DbContext(typeof(LibraryCollectionContext))]
    [Migration("20230306122713_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.V1LibraryEntry", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<DateTime?>("DateRead")
                        .HasColumnType("datetime")
                        .HasColumnName("date_read");

                    b.Property<string>("LibraryEntryISBN10")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasColumnName("isbn_10");

                    b.Property<string>("LibraryEntryISBN13")
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)")
                        .HasColumnName("isbn_13");

                    b.Property<string>("MainAuthor")
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("main_author");

                    b.Property<int?>("Rating")
                        .HasColumnType("int")
                        .HasColumnName("rating");

                    b.Property<string>("ReadingStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("reading_status");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("title");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(36)")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.ToTable("library_entries", "dbo");

                    b.HasData(
                        new
                        {
                            Id = "2d87b44e-20da-45a8-abdf-8296f251a680",
                            DateRead = new DateTime(2023, 2, 24, 12, 55, 19, 113, DateTimeKind.Unspecified),
                            LibraryEntryISBN10 = "1440674132",
                            LibraryEntryISBN13 = "9781440674136",
                            MainAuthor = "John Steinbeck",
                            Rating = 8,
                            ReadingStatus = "Completed",
                            Title = "The Moon Is Down",
                            UserId = "54af86bf-346a-4cba-b36f-527748e1cb93"
                        },
                        new
                        {
                            Id = "3bba26a9-3d8e-4f51-9ff4-1ad2d8da112b",
                            DateRead = new DateTime(2023, 1, 24, 11, 54, 29, 123, DateTimeKind.Unspecified),
                            LibraryEntryISBN10 = "1440674132",
                            LibraryEntryISBN13 = "9781440674136",
                            MainAuthor = "John Steinbeck",
                            Rating = 7,
                            ReadingStatus = "Completed",
                            Title = "The Moon Is Down",
                            UserId = "3fa85f64-5717-4562-b3fc-2c963f66afa6"
                        },
                        new
                        {
                            Id = "b77cc25f-68ed-40ab-9b0e-91ab588557f2",
                            LibraryEntryISBN10 = "1119797209",
                            LibraryEntryISBN13 = "9781119797203",
                            MainAuthor = "Christian Nagel",
                            ReadingStatus = "ToRead",
                            Title = "Professional C# and .NET",
                            UserId = "54af86bf-346a-4cba-b36f-527748e1cb93"
                        },
                        new
                        {
                            Id = "5c7629a7-bca3-481e-bddb-ffc263f7232a",
                            DateRead = new DateTime(2023, 2, 18, 8, 53, 21, 423, DateTimeKind.Unspecified),
                            LibraryEntryISBN10 = "0486415872",
                            LibraryEntryISBN13 = "9780486415871",
                            MainAuthor = "Fyodor Dostoyevsky",
                            Rating = 9,
                            ReadingStatus = "Completed",
                            Title = "Crime and Punishment",
                            UserId = "54af86bf-346a-4cba-b36f-527748e1cb93"
                        },
                        new
                        {
                            Id = "f26d0753-c47a-4745-9cd7-b207790617d0",
                            LibraryEntryISBN10 = "144810369X",
                            LibraryEntryISBN13 = "9781448103690",
                            MainAuthor = "Haruki Murakami",
                            ReadingStatus = "Reading",
                            Title = "Kafka on the Shore",
                            UserId = "e8cc12ba-4df6-4b06-b96e-9ad00a927a93"
                        },
                        new
                        {
                            Id = "8cae4a7d-a7e3-4d19-a20d-cb6b07641e95",
                            DateRead = new DateTime(2023, 2, 21, 7, 43, 11, 453, DateTimeKind.Unspecified),
                            LibraryEntryISBN10 = "144810369X",
                            LibraryEntryISBN13 = "9781448103690",
                            MainAuthor = "Haruki Murakami",
                            Rating = 10,
                            ReadingStatus = "Completed",
                            Title = "Kafka on the Shore",
                            UserId = "3fa85f64-5717-4562-b3fc-2c963f66afa6"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
