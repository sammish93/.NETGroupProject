﻿// <auto-generated />
using System;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Migrations.UserAccount
{
    [DbContext(typeof(UserAccountContext))]
    partial class UserAccountContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.V1LoginModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("password");

                    b.Property<string>("Salt")
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("salt");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("token");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.ToTable("login_verification", "dbo");

                    b.HasData(
                        new
                        {
                            Id = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                            Password = "70A1AF97C0496AD874DC",
                            Salt = "247432D4ED93DCE32929",
                            Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIzZmE4NWY2NC01NzE3LTQ1NjItYjNmYy0yYzk2M2Y2NmFmYTYiLCJuYmYiOjE2NzcyMzUxMDEsImV4cCI6MTY3NzQ5NDMwMSwiaWF0IjoxNjc3MjM1MTAxfQ.LqUQyhrnWwkNtkQUYcatydTdeAqvCaZZ4tEYovAGkJI",
                            UserName = "JinkxMonsoon"
                        },
                        new
                        {
                            Id = "54af86bf-346a-4cba-b36f-527748e1cb93",
                            Password = "A7E220F0781BE0C248A3",
                            Salt = "3E921C45F3A9089BDC7E",
                            Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI1NGFmODZiZi0zNDZhLTRjYmEtYjM2Zi01Mjc3NDhlMWNiOTMiLCJuYmYiOjE2NzcyMzU0OTQsImV4cCI6MTY3NzQ5NDY5NCwiaWF0IjoxNjc3MjM1NDk0fQ._VGOcvMVXmtj741AoUGLYnWsAvG5geuLHX_phvfOuT8",
                            UserName = "testaccount"
                        },
                        new
                        {
                            Id = "e8cc12ba-4df6-4b06-b96e-9ad00a927a93",
                            Password = "B1A8A1223DCA3A102726",
                            Salt = "A91F72A37D0E46037B85",
                            Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJlOGNjMTJiYS00ZGY2LTRiMDYtYjk2ZS05YWQwMGE5MjdhOTMiLCJuYmYiOjE2NzcyMzUyNDcsImV4cCI6MTY3NzQ5NDQ0NywiaWF0IjoxNjc3MjM1MjQ3fQ.NMCEXx8Dhr40krHQrpz4Zwgslj9N_HN3fi_Qrt4oMes",
                            UserName = "QueenOfTheNorth"
                        });
                });

            modelBuilder.Entity("Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.V1User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("city");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("country");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("first_name");

                    b.Property<string>("LangPreference")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("lang_preference");

                    b.Property<DateTime>("LastActive")
                        .HasColumnType("datetime")
                        .HasColumnName("last_active");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("last_name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("password");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime")
                        .HasColumnName("registration_date");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("user_role");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.ToTable("users", "dbo");

                    b.HasData(
                        new
                        {
                            Id = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                            City = "Seattle",
                            Country = "USA",
                            Email = "joojoo@gmail.com",
                            FirstName = "Jinkx",
                            LangPreference = "en",
                            LastActive = new DateTime(2023, 2, 24, 10, 37, 8, 387, DateTimeKind.Unspecified),
                            LastName = "Monsoon",
                            Password = "70A1AF97C0496AD874DC",
                            RegistrationDate = new DateTime(2023, 2, 24, 10, 37, 8, 387, DateTimeKind.Unspecified),
                            Role = "User",
                            UserName = "JinkxMonsoon"
                        },
                        new
                        {
                            Id = "e8cc12ba-4df6-4b06-b96e-9ad00a927a93",
                            City = "Toronto",
                            Country = "Canada",
                            Email = "b_hyteso@gmail.com",
                            FirstName = "Brooklyn",
                            LangPreference = "en",
                            LastActive = new DateTime(2023, 2, 24, 10, 39, 32, 540, DateTimeKind.Unspecified),
                            LastName = "Hytes",
                            Password = "B1A8A1223DCA3A102726",
                            RegistrationDate = new DateTime(2023, 2, 24, 10, 39, 32, 540, DateTimeKind.Unspecified),
                            Role = "User",
                            UserName = "QueenOfTheNorth"
                        },
                        new
                        {
                            Id = "54af86bf-346a-4cba-b36f-527748e1cb93",
                            City = "Oslo",
                            Country = "Norway",
                            Email = "testme@test.no",
                            FirstName = "Ola",
                            LangPreference = "no",
                            LastActive = new DateTime(2023, 2, 24, 10, 42, 49, 373, DateTimeKind.Unspecified),
                            LastName = "Nordmann",
                            Password = "A7E220F0781BE0C248A3",
                            RegistrationDate = new DateTime(2023, 2, 24, 10, 42, 49, 373, DateTimeKind.Unspecified),
                            Role = "Admin",
                            UserName = "testaccount"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
