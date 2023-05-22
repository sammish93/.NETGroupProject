using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Migrations
{
    /// <inheritdoc />
    public partial class AddSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "dbo",
                table: "marketplace_posts",
                column: "OwnerId",
                values: new object[]
                {
                    "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                    "54af86bf-346a-4cba-b36f-527748e1cb93"
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "marketplace_books",
                columns: new[] { "Id", "Condition", "Currency", "DateCreated", "DateModified", "ISBN10", "ISBN13", "OwnerId", "Price", "Status" },
                values: new object[,]
                {
                    { "2c775125-f0a8-46e0-bc3a-837c07414a56", "Bit of wear around the edges", "USD", new DateTime(2023, 4, 17, 8, 22, 20, 457, DateTimeKind.Unspecified), new DateTime(2023, 4, 17, 8, 22, 20, 457, DateTimeKind.Unspecified), "8771290060", "9788771290066", "54af86bf-346a-4cba-b36f-527748e1cb93", 20.0m, "UNSOLD" },
                    { "6b859b12-3001-43b5-bb4a-58155f07d63a", "used", "USD", new DateTime(2023, 5, 17, 8, 32, 42, 407, DateTimeKind.Unspecified), new DateTime(2023, 5, 17, 8, 32, 42, 407, DateTimeKind.Unspecified), "1410350339", "9781410350336", "3fa85f64-5717-4562-b3fc-2c963f66afa6", 8.0m, "UNSOLD" },
                    { "af728d4e-2879-4cda-bc6b-68101a65e191", "boka er helt nytt", "NOK", new DateTime(2023, 5, 17, 8, 22, 20, 457, DateTimeKind.Unspecified), new DateTime(2023, 5, 17, 8, 22, 20, 457, DateTimeKind.Unspecified), "8771290060", "9788771290066", "3fa85f64-5717-4562-b3fc-2c963f66afa6", 299.00m, "UNSOLD" },
                    { "cf6cb29f-0f29-44af-a0f0-a49cde7553ca", "new", "NOK", new DateTime(2023, 5, 16, 13, 15, 1, 870, DateTimeKind.Unspecified), new DateTime(2023, 5, 16, 13, 15, 1, 870, DateTimeKind.Unspecified), "0545919665", "9780545919661", "3fa85f64-5717-4562-b3fc-2c963f66afa6", 200.0m, "UNSOLD" },
                    { "cff03515-102b-4831-a599-fcde49b9b344", "new (packaged)", "EUR", new DateTime(2023, 5, 17, 12, 36, 59, 333, DateTimeKind.Unspecified), new DateTime(2023, 5, 17, 12, 36, 59, 333, DateTimeKind.Unspecified), "2808018630", "9782808018630", "3fa85f64-5717-4562-b3fc-2c963f66afa6", 25.00m, "UNSOLD" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "marketplace_books",
                keyColumn: "Id",
                keyValue: "2c775125-f0a8-46e0-bc3a-837c07414a56");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "marketplace_books",
                keyColumn: "Id",
                keyValue: "6b859b12-3001-43b5-bb4a-58155f07d63a");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "marketplace_books",
                keyColumn: "Id",
                keyValue: "af728d4e-2879-4cda-bc6b-68101a65e191");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "marketplace_books",
                keyColumn: "Id",
                keyValue: "cf6cb29f-0f29-44af-a0f0-a49cde7553ca");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "marketplace_books",
                keyColumn: "Id",
                keyValue: "cff03515-102b-4831-a599-fcde49b9b344");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "marketplace_posts",
                keyColumn: "OwnerId",
                keyValue: "3fa85f64-5717-4562-b3fc-2c963f66afa6");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "marketplace_posts",
                keyColumn: "OwnerId",
                keyValue: "54af86bf-346a-4cba-b36f-527748e1cb93");
        }
    }
}
