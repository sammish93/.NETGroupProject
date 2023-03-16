using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Migrations.UserIdentity
{
    /// <inheritdoc />
    public partial class AddedRolesToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "05579238-4da6-46be-a9b3-20d46fe9ad99", null, "Author", "AUTOR" },
                    { "4d471c38-89f4-497f-86d6-dfa456ba4653", null, "Admin", "ADMIN" },
                    { "8ed5b6a2-ef6d-4ad5-9cde-d3383b6c8657", null, "User", "USER" },
                    { "b333e30c-485d-4a18-a9a3-c06d6ef6f797", null, "Moderator", "MODERATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "05579238-4da6-46be-a9b3-20d46fe9ad99");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d471c38-89f4-497f-86d6-dfa456ba4653");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8ed5b6a2-ef6d-4ad5-9cde-d3383b6c8657");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b333e30c-485d-4a18-a9a3-c06d6ef6f797");
        }
    }
}
