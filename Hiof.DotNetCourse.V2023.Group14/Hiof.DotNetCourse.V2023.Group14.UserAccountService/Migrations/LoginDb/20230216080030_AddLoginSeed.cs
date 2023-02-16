using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Migrations.LoginDb
{
    /// <inheritdoc />
    public partial class AddLoginSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "dbo",
                table: "LoginVerification",
                columns: new[] { "Id", "Password", "Salt", "Token", "UserName" },
                values: new object[] { 1, "86D4CF04EDF276BA6AF1", "20OVaK6glLyqxg==", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI1IiwibmJmIjoxNjc2Mzg2MDMyLCJleHAiOjE2NzY2NDUyMzIsImlhdCI6MTY3NjM4NjAzMn0.q4akzYxENnXvykOPEYCqqZ9tSPT0fnoIvPtfFAs5IwQ", "stian" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LoginVerification",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
