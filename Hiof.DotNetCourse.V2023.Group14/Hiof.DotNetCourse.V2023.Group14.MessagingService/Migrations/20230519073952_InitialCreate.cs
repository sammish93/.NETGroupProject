using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hiof.DotNetCourse.V2023.Group14.MessagingService.Migrations
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
                name: "conversations",
                schema: "dbo",
                columns: table => new
                {
                    ConversationId = table.Column<string>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conversations", x => x.ConversationId);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                schema: "dbo",
                columns: table => new
                {
                    MessageId = table.Column<string>(type: "char(36)", nullable: false),
                    Sender = table.Column<string>(type: "varchar(255)", nullable: false),
                    Message = table.Column<string>(type: "varchar(1000)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConversationId = table.Column<string>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_messages_conversations",
                        column: x => x.ConversationId,
                        principalSchema: "dbo",
                        principalTable: "conversations",
                        principalColumn: "ConversationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "participants",
                schema: "dbo",
                columns: table => new
                {
                    Participant = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConversationId = table.Column<string>(type: "char(36)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_participants", x => new { x.Participant, x.ConversationId });
                    table.ForeignKey(
                        name: "FK_participants_conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalSchema: "dbo",
                        principalTable: "conversations",
                        principalColumn: "ConversationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "message_reactions",
                schema: "dbo",
                columns: table => new
                {
                    ReactionId = table.Column<string>(type: "char(36)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    MessageId = table.Column<string>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_message_reactions", x => x.ReactionId);
                    table.ForeignKey(
                        name: "FK_reactions_messages",
                        column: x => x.MessageId,
                        principalSchema: "dbo",
                        principalTable: "messages",
                        principalColumn: "MessageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_message_reactions_MessageId",
                schema: "dbo",
                table: "message_reactions",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_messages_ConversationId",
                schema: "dbo",
                table: "messages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_participants_ConversationId",
                schema: "dbo",
                table: "participants",
                column: "ConversationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "message_reactions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "participants",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "messages",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "conversations",
                schema: "dbo");
        }
    }
}
