using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hiof.DotNetCourse.V2023.Group14.MessagingService.Migrations
{
    /// <inheritdoc />
    public partial class AddSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "dbo",
                table: "conversations",
                column: "ConversationId",
                values: new object[]
                {
                    "34eef5ed-35be-408a-a562-95ea91f24fd1",
                    "3ad21e84-4ed0-4483-8ca7-1b29ae06fa00"
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "messages",
                columns: new[] { "MessageId", "ConversationId", "Date", "Message", "Sender" },
                values: new object[,]
                {
                    { "71456bc4-16d4-4b26-9f55-9ce88fa179f6", "34eef5ed-35be-408a-a562-95ea91f24fd1", new DateTime(2023, 5, 19, 11, 13, 28, 830, DateTimeKind.Unspecified).AddTicks(9960), "hey!", "3fa85f64-5717-4562-b3fc-2c963f66afa6" },
                    { "b24c9d48-79a3-4ef3-bb8e-a978ac4a7170", "34eef5ed-35be-408a-a562-95ea91f24fd1", new DateTime(2023, 5, 19, 11, 9, 59, 399, DateTimeKind.Unspecified).AddTicks(5659), "Hello there :)", "54af86bf-346a-4cba-b36f-527748e1cb93" },
                    { "d0d316e0-10f3-4460-8571-35c23c3ae15b", "34eef5ed-35be-408a-a562-95ea91f24fd1", new DateTime(2023, 5, 19, 11, 15, 9, 709, DateTimeKind.Unspecified).AddTicks(6212), "nice to hear from you", "54af86bf-346a-4cba-b36f-527748e1cb93" },
                    { "e3d9836a-208f-4e04-9e22-63b43e6f76e9", "3ad21e84-4ed0-4483-8ca7-1b29ae06fa00", new DateTime(2023, 5, 19, 8, 35, 18, 894, DateTimeKind.Unspecified).AddTicks(6909), "Hello!!", "3fa85f64-5717-4562-b3fc-2c963f66afa6" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "participants",
                columns: new[] { "ConversationId", "Participant", "IsRead" },
                values: new object[,]
                {
                    { "34eef5ed-35be-408a-a562-95ea91f24fd1", "3fa85f64-5717-4562-b3fc-2c963f66afa6", false },
                    { "3ad21e84-4ed0-4483-8ca7-1b29ae06fa00", "3fa85f64-5717-4562-b3fc-2c963f66afa6", true },
                    { "34eef5ed-35be-408a-a562-95ea91f24fd1", "54af86bf-346a-4cba-b36f-527748e1cb93", true },
                    { "3ad21e84-4ed0-4483-8ca7-1b29ae06fa00", "e8cc12ba-4df6-4b06-b96e-9ad00a927a93", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "messages",
                keyColumn: "MessageId",
                keyValue: "71456bc4-16d4-4b26-9f55-9ce88fa179f6");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "messages",
                keyColumn: "MessageId",
                keyValue: "b24c9d48-79a3-4ef3-bb8e-a978ac4a7170");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "messages",
                keyColumn: "MessageId",
                keyValue: "d0d316e0-10f3-4460-8571-35c23c3ae15b");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "messages",
                keyColumn: "MessageId",
                keyValue: "e3d9836a-208f-4e04-9e22-63b43e6f76e9");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "participants",
                keyColumns: new[] { "ConversationId", "Participant" },
                keyValues: new object[] { "34eef5ed-35be-408a-a562-95ea91f24fd1", "3fa85f64-5717-4562-b3fc-2c963f66afa6" });

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "participants",
                keyColumns: new[] { "ConversationId", "Participant" },
                keyValues: new object[] { "3ad21e84-4ed0-4483-8ca7-1b29ae06fa00", "3fa85f64-5717-4562-b3fc-2c963f66afa6" });

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "participants",
                keyColumns: new[] { "ConversationId", "Participant" },
                keyValues: new object[] { "34eef5ed-35be-408a-a562-95ea91f24fd1", "54af86bf-346a-4cba-b36f-527748e1cb93" });

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "participants",
                keyColumns: new[] { "ConversationId", "Participant" },
                keyValues: new object[] { "3ad21e84-4ed0-4483-8ca7-1b29ae06fa00", "e8cc12ba-4df6-4b06-b96e-9ad00a927a93" });

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "conversations",
                keyColumn: "ConversationId",
                keyValue: "34eef5ed-35be-408a-a562-95ea91f24fd1");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "conversations",
                keyColumn: "ConversationId",
                keyValue: "3ad21e84-4ed0-4483-8ca7-1b29ae06fa00");
        }
    }
}
