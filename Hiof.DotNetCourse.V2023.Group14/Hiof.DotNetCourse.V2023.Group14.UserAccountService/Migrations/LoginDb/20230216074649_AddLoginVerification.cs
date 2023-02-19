using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Migrations.LoginDb
{
    /// <inheritdoc />
    public partial class AddLoginVerification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema( 
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "LoginVerification",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    Salt = table.Column<string>(type: "nvarchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginVerification", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginVerification",
                schema: "dbo");
        }
    }
}
