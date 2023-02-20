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

            modelBuilder.Entity("Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.V1User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("City");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("Country");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("FirstName");

                    b.Property<string>("LangPreference")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("LangPreference");

                    b.Property<DateTime>("LastActive")
                        .HasColumnType("datetime")
                        .HasColumnName("LastActive");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("lastName");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("password");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime")
                        .HasColumnName("RegistrationDate");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("UserRole");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("UserName");

                    b.HasKey("Id");

                    b.ToTable("Users", "dbo");
                });
#pragma warning restore 612, 618
        }
    }
}
