using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hiof.DotNetCourse.V2023.Group14.ReadingGoalService.Migrations
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
                name: "reading_goals",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(36)", nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(36)", nullable: false),
                    goal_start = table.Column<DateTime>(type: "datetime", nullable: false),
                    goal_end = table.Column<DateTime>(type: "datetime", nullable: false),
                    goal_target = table.Column<int>(type: "int", nullable: false),
                    goal_curr = table.Column<int>(type: "int", nullable: false),
                    last_updated = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reading_goals", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "reading_goals",
                columns: new[] { "id", "goal_curr", "goal_end", "goal_start", "goal_target", "last_updated", "user_id" },
                values: new object[,]
                {
                    { "1dd99ec1-aaae-4df5-b9e2-fc233a9e1dd9", 5, new DateTime(2023, 3, 21, 7, 43, 11, 453, DateTimeKind.Unspecified), new DateTime(2023, 2, 21, 7, 43, 11, 453, DateTimeKind.Unspecified), 20, new DateTime(2023, 2, 21, 7, 43, 11, 453, DateTimeKind.Unspecified), "3fa85f64-5717-4562-b3fc-2c963f66afa6" },
                    { "273b0fd5-cf41-4089-8611-f41d8099992c", 7, new DateTime(2023, 4, 21, 7, 43, 11, 453, DateTimeKind.Unspecified), new DateTime(2023, 1, 21, 7, 43, 11, 453, DateTimeKind.Unspecified), 20, new DateTime(2023, 2, 21, 11, 54, 29, 123, DateTimeKind.Unspecified), "54af86bf-346a-4cba-b36f-527748e1cb93" },
                    { "3b6c06da-250e-4f7a-b867-facbc6a48574", 17, new DateTime(2022, 12, 21, 7, 43, 11, 453, DateTimeKind.Unspecified), new DateTime(2022, 10, 21, 7, 43, 11, 453, DateTimeKind.Unspecified), 20, new DateTime(2023, 2, 21, 7, 43, 11, 453, DateTimeKind.Unspecified), "3fa85f64-5717-4562-b3fc-2c963f66afa6" },
                    { "c6050d5c-8b0a-402a-a020-7c2c4e82584a", 1, new DateTime(2022, 7, 21, 7, 43, 11, 453, DateTimeKind.Unspecified), new DateTime(2022, 5, 21, 7, 43, 11, 453, DateTimeKind.Unspecified), 15, new DateTime(2023, 6, 21, 7, 43, 11, 453, DateTimeKind.Unspecified), "54af86bf-346a-4cba-b36f-527748e1cb93" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reading_goals",
                schema: "dbo");
        }
    }
}
