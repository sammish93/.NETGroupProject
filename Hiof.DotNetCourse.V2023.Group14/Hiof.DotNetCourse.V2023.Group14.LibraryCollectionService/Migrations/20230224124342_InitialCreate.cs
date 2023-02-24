using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

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
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    isbn_10 = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    isbn_13 = table.Column<string>(type: "nvarchar(13)", nullable: true),
                    title = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    main_author = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: true),
                    date_read = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_library_entries", x => x.id);
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
