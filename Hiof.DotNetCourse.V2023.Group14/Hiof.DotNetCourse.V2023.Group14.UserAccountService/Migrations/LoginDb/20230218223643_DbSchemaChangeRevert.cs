using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Migrations.LoginDb
{
    /// <inheritdoc />
    public partial class DbSchemaChangeRevert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "LoginVerification",
                schema: "db",
                newName: "LoginVerification",
                newSchema: "dbo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "db");

            migrationBuilder.RenameTable(
                name: "LoginVerification",
                schema: "dbo",
                newName: "LoginVerification",
                newSchema: "db");
        }
    }
}
