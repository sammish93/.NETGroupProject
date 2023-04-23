using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hiof.DotNetCourse.V2023.Group14.CommentService.Migrations
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
                name: "user_comments",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(36)", nullable: false),
                    body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    upvotes = table.Column<int>(type: "int", nullable: true),
                    author_id = table.Column<string>(type: "char(36)", nullable: false),
                    parent_comment_id = table.Column<string>(type: "char(36)", nullable: true),
                    comment_type = table.Column<int>(type: "int", nullable: false),
                    isbn_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isbn_13 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_id = table.Column<string>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_comments", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_comments_user_comments_parent_comment_id",
                        column: x => x.parent_comment_id,
                        principalSchema: "dbo",
                        principalTable: "user_comments",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "user_comments",
                columns: new[] { "id", "author_id", "body", "comment_type", "created_at", "isbn_10", "isbn_13", "parent_comment_id", "upvotes", "user_id" },
                values: new object[,]
                {
                    { "48dca027-f9c8-4c92-bbf9-db1dfdcc0a00", "54af86bf-346a-4cba-b36f-527748e1cb93", "Great collection of books!", 0, new DateTime(2023, 2, 27, 11, 55, 19, 113, DateTimeKind.Unspecified), null, null, null, 0, "3fa85f64-5717-4562-b3fc-2c963f66afa6" },
                    { "4a14dc4d-aa39-4ee5-bc34-b46701c3ca09", "3fa85f64-5717-4562-b3fc-2c963f66afa6", "This book was good. At times it was a bit boring and difficult to read.", 1, new DateTime(2023, 2, 24, 12, 55, 19, 113, DateTimeKind.Unspecified), "1440674132", "9781440674136", null, 0, null },
                    { "8f899b47-6229-4795-8cba-ba644a479d55", "e8cc12ba-4df6-4b06-b96e-9ad00a927a93", "It was an okay book", 1, new DateTime(2023, 2, 25, 11, 59, 19, 113, DateTimeKind.Unspecified), "1440674132", "9781440674136", null, 0, null },
                    { "0e6145cc-4150-43a8-ae24-81e8a69fad7f", "54af86bf-346a-4cba-b36f-527748e1cb93", "I agree with how difficult it was to read at times", 2, new DateTime(2023, 2, 25, 11, 55, 19, 113, DateTimeKind.Unspecified), null, null, "4a14dc4d-aa39-4ee5-bc34-b46701c3ca09", 0, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_comments_parent_comment_id",
                schema: "dbo",
                table: "user_comments",
                column: "parent_comment_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_comments",
                schema: "dbo");
        }
    }
}
