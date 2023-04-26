using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Migrations
{
    /// <inheritdoc />
    public partial class CreateMarketplaceTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "marketplace_posts",
                schema: "dbo",
                columns: table => new
                {
                    OwnerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marketplace_posts", x => x.OwnerId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "marketplace_books",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Condition = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime", nullable: false),
                    V1MarketplaceUserOwnerId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marketplace_books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_marketplace_books_marketplace_posts_V1MarketplaceUserOwnerId",
                        column: x => x.V1MarketplaceUserOwnerId,
                        principalSchema: "dbo",
                        principalTable: "marketplace_posts",
                        principalColumn: "OwnerId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_marketplace_books_V1MarketplaceUserOwnerId",
                schema: "dbo",
                table: "marketplace_books",
                column: "V1MarketplaceUserOwnerId");
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
