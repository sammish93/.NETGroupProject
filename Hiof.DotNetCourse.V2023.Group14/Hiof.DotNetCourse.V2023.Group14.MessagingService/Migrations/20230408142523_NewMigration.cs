using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hiof.DotNetCourse.V2023.Group14.MessagingService.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_messages_conversations_V1ConversationModelConversationId",
                schema: "dbo",
                table: "messages");

            migrationBuilder.DropIndex(
                name: "IX_messages_V1ConversationModelConversationId",
                schema: "dbo",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "V1ConversationModelConversationId",
                schema: "dbo",
                table: "messages");

            migrationBuilder.CreateIndex(
                name: "IX_messages_ConversationId",
                schema: "dbo",
                table: "messages",
                column: "ConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_messages_conversations_ConversationId",
                schema: "dbo",
                table: "messages",
                column: "ConversationId",
                principalSchema: "dbo",
                principalTable: "conversations",
                principalColumn: "ConversationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_messages_conversations_ConversationId",
                schema: "dbo",
                table: "messages");

            migrationBuilder.DropIndex(
                name: "IX_messages_ConversationId",
                schema: "dbo",
                table: "messages");

            migrationBuilder.AddColumn<Guid>(
                name: "V1ConversationModelConversationId",
                schema: "dbo",
                table: "messages",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_messages_V1ConversationModelConversationId",
                schema: "dbo",
                table: "messages",
                column: "V1ConversationModelConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_messages_conversations_V1ConversationModelConversationId",
                schema: "dbo",
                table: "messages",
                column: "V1ConversationModelConversationId",
                principalSchema: "dbo",
                principalTable: "conversations",
                principalColumn: "ConversationId");
        }
    }
}
