using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Migrations.UserAccount
{
    /// <inheritdoc />
    public partial class Seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "dbo",
                table: "users",
                columns: new[] { "id", "city", "country", "email", "first_name", "lang_preference", "last_active", "last_name", "password", "registration_date", "user_role", "username" },
                values: new object[] { new Guid("a2e9ee22-e515-48cc-8ea0-7229a1c7c590"), "Seattle", "USA", "joojoo@gmail.com", "Jinkx", "en", new DateTime(2023, 2, 23, 10, 48, 1, 380, DateTimeKind.Local).AddTicks(4764), "Monsoon", "Itismonsoonseason!", new DateTime(2023, 2, 23, 10, 48, 1, 380, DateTimeKind.Local).AddTicks(4721), "User", "JinkxMonsoon" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("a2e9ee22-e515-48cc-8ea0-7229a1c7c590"));
        }
    }
}
