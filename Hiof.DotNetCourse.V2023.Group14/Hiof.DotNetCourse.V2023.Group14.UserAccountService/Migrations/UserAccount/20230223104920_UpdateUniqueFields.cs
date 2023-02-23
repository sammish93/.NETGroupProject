using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Migrations.UserAccount
{
    /// <inheritdoc />
    public partial class UpdateUniqueFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { new Guid("4cf318a5-95d1-4231-850f-a3aef61aaadd"), "Toronto", "Canada", "b_hyteso@gmail.com", "Brooklyn", "en", new DateTime(2023, 2, 23, 11, 49, 20, 14, DateTimeKind.Local).AddTicks(8066), "Hytes", "CanadianTurkeyBacon", new DateTime(2023, 2, 23, 11, 49, 20, 14, DateTimeKind.Local).AddTicks(8064), "User", "QueenOfTheNorth" },
                    { new Guid("d9813480-cc17-4b2e-a749-626a16a1d096"), "Palm Springs", "USA", "bdelrio@yahoo.com", "Bianca", "en", new DateTime(2023, 2, 23, 11, 49, 20, 14, DateTimeKind.Local).AddTicks(8069), "Del Rio", "Baloney2123", new DateTime(2023, 2, 23, 11, 49, 20, 14, DateTimeKind.Local).AddTicks(8068), "Admin", "ClownBeauty" },
                    { new Guid("dc1dcbf9-3414-4f15-8745-6ebeada93861"), "Seattle", "USA", "joojoo@gmail.com", "Jinkx", "en", new DateTime(2023, 2, 23, 11, 49, 20, 14, DateTimeKind.Local).AddTicks(8060), "Monsoon", "Itismonsoonseason!", new DateTime(2023, 2, 23, 11, 49, 20, 14, DateTimeKind.Local).AddTicks(8010), "User", "JinkxMonsoon" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                schema: "dbo",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                schema: "dbo",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_email",
                schema: "dbo",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_username",
                schema: "dbo",
                table: "users");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("4cf318a5-95d1-4231-850f-a3aef61aaadd"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("d9813480-cc17-4b2e-a749-626a16a1d096"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("dc1dcbf9-3414-4f15-8745-6ebeada93861"));

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
    }
}
