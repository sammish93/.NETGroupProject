using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Migrations.UserAccount
{
    /// <inheritdoc />
    public partial class UniqueFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[,]
                {
                    { new Guid("04303abe-0c95-40ad-b5e8-b2fa0aaaf985"), "Palm Springs", "USA", "bdelrio@yahoo.com", "Bianca", "en", new DateTime(2023, 2, 23, 11, 32, 23, 253, DateTimeKind.Local).AddTicks(211), "Del Rio", "Baloney2123", new DateTime(2023, 2, 23, 11, 32, 23, 253, DateTimeKind.Local).AddTicks(210), "Admin", "ClownBeauty" },
                    { new Guid("6f88f584-766f-48ab-b5a1-78770b89c337"), "Toronto", "Canada", "b_hyteso@gmail.com", "Brooklyn", "en", new DateTime(2023, 2, 23, 11, 32, 23, 253, DateTimeKind.Local).AddTicks(207), "Hytes", "CanadianTurkeyBacon", new DateTime(2023, 2, 23, 11, 32, 23, 253, DateTimeKind.Local).AddTicks(206), "User", "QueenOfTheNorth" },
                    { new Guid("f77da20a-9f9b-45e9-8bc6-d2c0e6acdd86"), "Seattle", "USA", "joojoo@gmail.com", "Jinkx", "en", new DateTime(2023, 2, 23, 11, 32, 23, 253, DateTimeKind.Local).AddTicks(200), "Monsoon", "Itismonsoonseason!", new DateTime(2023, 2, 23, 11, 32, 23, 253, DateTimeKind.Local).AddTicks(160), "User", "JinkxMonsoon" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_email_username",
                schema: "dbo",
                table: "users",
                columns: new[] { "email", "username" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_email_username",
                schema: "dbo",
                table: "users");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("04303abe-0c95-40ad-b5e8-b2fa0aaaf985"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("6f88f584-766f-48ab-b5a1-78770b89c337"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("f77da20a-9f9b-45e9-8bc6-d2c0e6acdd86"));

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
    }
}
