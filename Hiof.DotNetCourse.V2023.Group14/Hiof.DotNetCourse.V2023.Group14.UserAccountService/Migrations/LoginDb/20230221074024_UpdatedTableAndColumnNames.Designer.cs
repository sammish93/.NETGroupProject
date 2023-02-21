﻿// <auto-generated />
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Migrations.LoginDb
{
    [DbContext(typeof(LoginDbContext))]
    [Migration("20230221074024_UpdatedTableAndColumnNames")]
    partial class UpdatedTableAndColumnNames
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.V1LoginModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

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
                            Id = 1,
                            Password = "86D4CF04EDF276BA6AF1",
                            Salt = "20OVaK6glLyqxg==",
                            Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI1IiwibmJmIjoxNjc2Mzg2MDMyLCJleHAiOjE2NzY2NDUyMzIsImlhdCI6MTY3NjM4NjAzMn0.q4akzYxENnXvykOPEYCqqZ9tSPT0fnoIvPtfFAs5IwQ",
                            UserName = "stian"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
