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

            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "conversations",
                schema: "dbo",
                columns: table => new
                {
                    ConversationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conversations", x => x.ConversationId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "messages",
                schema: "dbo",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Sender = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Message = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ConversationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    V1ConversationModelConversationId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_messages_conversations_V1ConversationModelConversationId",
                        column: x => x.V1ConversationModelConversationId,
                        principalSchema: "dbo",
                        principalTable: "conversations",
                        principalColumn: "ConversationId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "participants",
                schema: "dbo",
                columns: table => new
                {
                    Participant = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConversationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "message_reaction",
                schema: "dbo",
                columns: table => new
                {
                    ReactionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MessageId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    V1MessagesMessageId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_message_reaction", x => x.ReactionId);
                    table.ForeignKey(
                        name: "FK_message_reaction_messages_V1MessagesMessageId",
                        column: x => x.V1MessagesMessageId,
                        principalSchema: "dbo",
                        principalTable: "messages",
                        principalColumn: "MessageId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_message_reaction_V1MessagesMessageId",
                schema: "dbo",
                table: "message_reaction",
                column: "V1MessagesMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_messages_V1ConversationModelConversationId",
                schema: "dbo",
                table: "messages",
                column: "V1ConversationModelConversationId");

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
                name: "message_reaction",
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
