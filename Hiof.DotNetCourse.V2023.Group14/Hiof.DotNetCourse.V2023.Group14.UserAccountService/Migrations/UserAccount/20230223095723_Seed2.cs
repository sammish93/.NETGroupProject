using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Migrations.UserAccount
{
    /// <inheritdoc />
    public partial class Seed2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("a2e9ee22-e515-48cc-8ea0-7229a1c7c590"));

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "users",
                columns: new[] { "id", "city", "country", "email", "first_name", "lang_preference", "last_active", "last_name", "password", "registration_date", "user_role", "username" },
                values: new object[,]
                {
                    { new Guid("59890f58-7bce-47d1-a308-56a72f58c80a"), "Seattle", "USA", "joojoo@gmail.com", "Jinkx", "en", new DateTime(2023, 2, 23, 10, 57, 23, 541, DateTimeKind.Local).AddTicks(6348), "Monsoon", "Itismonsoonseason!", new DateTime(2023, 2, 23, 10, 57, 23, 541, DateTimeKind.Local).AddTicks(6307), "User", "JinkxMonsoon" },
                    { new Guid("7a5e9c16-e8b0-498a-bbce-c674503346e9"), "Palm Springs", "USA", "bdelrio@yahoo.com", "Bianca", "en", new DateTime(2023, 2, 23, 10, 57, 23, 541, DateTimeKind.Local).AddTicks(6358), "Del Rio", "Baloney2123", new DateTime(2023, 2, 23, 10, 57, 23, 541, DateTimeKind.Local).AddTicks(6357), "Admin", "ClownBeauty" },
                    { new Guid("e43fdf2b-523c-471d-8664-97c9d9ad6c11"), "Toronto", "Canada", "b_hyteso@gmail.com", "Brooklyn", "en", new DateTime(2023, 2, 23, 10, 57, 23, 541, DateTimeKind.Local).AddTicks(6354), "Hytes", "CanadianTurkeyBacon", new DateTime(2023, 2, 23, 10, 57, 23, 541, DateTimeKind.Local).AddTicks(6353), "User", "QueenOfTheNorth" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("59890f58-7bce-47d1-a308-56a72f58c80a"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("7a5e9c16-e8b0-498a-bbce-c674503346e9"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("e43fdf2b-523c-471d-8664-97c9d9ad6c11"));

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "users",
                columns: new[] { "id", "city", "country", "email", "first_name", "lang_preference", "last_active", "last_name", "password", "registration_date", "user_role", "username" },
                values: new object[] { new Guid("a2e9ee22-e515-48cc-8ea0-7229a1c7c590"), "Seattle", "USA", "joojoo@gmail.com", "Jinkx", "en", new DateTime(2023, 2, 23, 10, 48, 1, 380, DateTimeKind.Local).AddTicks(4764), "Monsoon", "Itismonsoonseason!", new DateTime(2023, 2, 23, 10, 48, 1, 380, DateTimeKind.Local).AddTicks(4721), "User", "JinkxMonsoon" });
        }
    }
}
