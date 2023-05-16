using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Migrations
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
                name: "marketplace_posts",
                schema: "dbo",
                columns: table => new
                {
                    OwnerId = table.Column<string>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marketplace_posts", x => x.OwnerId);
                });

            migrationBuilder.CreateTable(
                name: "marketplace_books",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", nullable: false),
                    Condition = table.Column<string>(type: "varchar(255)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Currency = table.Column<string>(type: "varchar(50)", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    OwnerId = table.Column<string>(type: "char(36)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime", nullable: false),
                    ISBN10 = table.Column<string>(type: "varchar(10)", nullable: true),
                    ISBN13 = table.Column<string>(type: "varchar(13)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marketplace_books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_marketplace_owner",
                        column: x => x.OwnerId,
                        principalSchema: "dbo",
                        principalTable: "marketplace_posts",
                        principalColumn: "OwnerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_marketplace_books_OwnerId",
                schema: "dbo",
                table: "marketplace_books",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "marketplace_books",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "marketplace_posts",
                schema: "dbo");
        }
    }
}
