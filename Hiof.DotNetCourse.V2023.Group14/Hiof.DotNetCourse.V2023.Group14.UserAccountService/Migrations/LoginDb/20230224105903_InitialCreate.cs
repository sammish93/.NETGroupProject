using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Migrations.LoginDb
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
                name: "login_verification",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "nvarchar(36)", nullable: false),
                    username = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    salt = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    token = table.Column<string>(type: "nvarchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_login_verification", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "login_verification",
                schema: "dbo");
        }
    }
}
