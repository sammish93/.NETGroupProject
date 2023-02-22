using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Migrations.LoginDb
{
    /// <inheritdoc />
    public partial class UpdatedTableAndColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginVerification",
                schema: "dbo");

            migrationBuilder.CreateTable(
                name: "login_verification",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    token = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    salt = table.Column<string>(type: "nvarchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_login_verification", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "login_verification",
                columns: new[] { "id", "password", "salt", "token", "username" },
                values: new object[] { 1, "86D4CF04EDF276BA6AF1", "20OVaK6glLyqxg==", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI1IiwibmJmIjoxNjc2Mzg2MDMyLCJleHAiOjE2NzY2NDUyMzIsImlhdCI6MTY3NjM4NjAzMn0.q4akzYxENnXvykOPEYCqqZ9tSPT0fnoIvPtfFAs5IwQ", "stian" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "login_verification",
                schema: "dbo");

            migrationBuilder.CreateTable(
                name: "LoginVerification",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginVerification", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "LoginVerification",
                columns: new[] { "Id", "Password", "Salt", "Token", "UserName" },
                values: new object[] { 1, "86D4CF04EDF276BA6AF1", "20OVaK6glLyqxg==", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI1IiwibmJmIjoxNjc2Mzg2MDMyLCJleHAiOjE2NzY2NDUyMzIsImlhdCI6MTY3NjM4NjAzMn0.q4akzYxENnXvykOPEYCqqZ9tSPT0fnoIvPtfFAs5IwQ", "stian" });
        }
    }
}
