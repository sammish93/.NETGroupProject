using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Migrations
{
    /// <inheritdoc />
    public partial class SeedTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Tests",
                columns: new[] { "id", "age", "full_name" },
                values: new object[,]
                {
                    { 1, 42, "Jonas" },
                    { 2, 1, "Dobby the House Dog" },
                    { 3, 743, "Margaret of Anjou" },
                    { 4, 29, "Sam" },
                    { 5, 64, "Squiggle" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Tests",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Tests",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Tests",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Tests",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Tests",
                keyColumn: "id",
                keyValue: 5);
        }
    }
}
