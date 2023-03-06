using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hiof.DotNetCourse.V2023.Group14.LibraryCollectionService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "library_entries",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(36)", nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(36)", nullable: false),
                    isbn_10 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    isbn_13 = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    title = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    main_author = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    rating = table.Column<int>(type: "int", nullable: true),
                    date_read = table.Column<DateTime>(type: "datetime", nullable: true),
                    reading_status = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_library_entries", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "library_entries",
                columns: new[] { "id", "date_read", "isbn_10", "isbn_13", "main_author", "rating", "reading_status", "title", "user_id" },
                values: new object[,]
                {
                    { "2d87b44e-20da-45a8-abdf-8296f251a680", new DateTime(2023, 2, 24, 12, 55, 19, 113, DateTimeKind.Unspecified), "1440674132", "9781440674136", "John Steinbeck", 8, "Completed", "The Moon Is Down", "54af86bf-346a-4cba-b36f-527748e1cb93" },
                    { "3bba26a9-3d8e-4f51-9ff4-1ad2d8da112b", new DateTime(2023, 1, 24, 11, 54, 29, 123, DateTimeKind.Unspecified), "1440674132", "9781440674136", "John Steinbeck", 7, "Completed", "The Moon Is Down", "3fa85f64-5717-4562-b3fc-2c963f66afa6" },
                    { "5c7629a7-bca3-481e-bddb-ffc263f7232a", new DateTime(2023, 2, 18, 8, 53, 21, 423, DateTimeKind.Unspecified), "0486415872", "9780486415871", "Fyodor Dostoyevsky", 9, "Completed", "Crime and Punishment", "54af86bf-346a-4cba-b36f-527748e1cb93" },
                    { "8cae4a7d-a7e3-4d19-a20d-cb6b07641e95", new DateTime(2023, 2, 21, 7, 43, 11, 453, DateTimeKind.Unspecified), "144810369X", "9781448103690", "Haruki Murakami", 10, "Completed", "Kafka on the Shore", "3fa85f64-5717-4562-b3fc-2c963f66afa6" },
                    { "b77cc25f-68ed-40ab-9b0e-91ab588557f2", null, "1119797209", "9781119797203", "Christian Nagel", null, "ToRead", "Professional C# and .NET", "54af86bf-346a-4cba-b36f-527748e1cb93" },
                    { "f26d0753-c47a-4745-9cd7-b207790617d0", null, "144810369X", "9781448103690", "Haruki Murakami", null, "Reading", "Kafka on the Shore", "e8cc12ba-4df6-4b06-b96e-9ad00a927a93" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "library_entries",
                schema: "dbo");
        }
    }
}
