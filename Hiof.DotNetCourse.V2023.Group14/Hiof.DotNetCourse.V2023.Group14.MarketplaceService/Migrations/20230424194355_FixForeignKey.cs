using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Migrations
{
    /// <inheritdoc />
    public partial class FixForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_marketplace_books_marketplace_posts_V1MarketplaceUserOwnerId",
                schema: "dbo",
                table: "marketplace_books");

            migrationBuilder.DropIndex(
                name: "IX_marketplace_books_V1MarketplaceUserOwnerId",
                schema: "dbo",
                table: "marketplace_books");

            migrationBuilder.DropColumn(
                name: "V1MarketplaceUserOwnerId",
                schema: "dbo",
                table: "marketplace_books");

            migrationBuilder.CreateIndex(
                name: "IX_marketplace_books_OwnerId",
                schema: "dbo",
                table: "marketplace_books",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_marketplace_owner",
                schema: "dbo",
                table: "marketplace_books",
                column: "OwnerId",
                principalSchema: "dbo",
                principalTable: "marketplace_posts",
                principalColumn: "OwnerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_marketplace_owner",
                schema: "dbo",
                table: "marketplace_books");

            migrationBuilder.DropIndex(
                name: "IX_marketplace_books_OwnerId",
                schema: "dbo",
                table: "marketplace_books");

            migrationBuilder.AddColumn<Guid>(
                name: "V1MarketplaceUserOwnerId",
                schema: "dbo",
                table: "marketplace_books",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_marketplace_books_V1MarketplaceUserOwnerId",
                schema: "dbo",
                table: "marketplace_books",
                column: "V1MarketplaceUserOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_marketplace_books_marketplace_posts_V1MarketplaceUserOwnerId",
                schema: "dbo",
                table: "marketplace_books",
                column: "V1MarketplaceUserOwnerId",
                principalSchema: "dbo",
                principalTable: "marketplace_posts",
                principalColumn: "OwnerId");
        }
    }
}
