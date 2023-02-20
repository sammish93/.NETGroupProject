using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Migrations.UserAccount
{
    /// <inheritdoc />
    public partial class DbChangeForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "loginModelId",
                schema: "dbo",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LoginVerification",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginVerification", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_loginModelId",
                schema: "dbo",
                table: "Users",
                column: "loginModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_LoginVerification_loginModelId",
                schema: "dbo",
                table: "Users",
                column: "loginModelId",
                principalSchema: "dbo",
                principalTable: "LoginVerification",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_LoginVerification_loginModelId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropTable(
                name: "LoginVerification",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Users_loginModelId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "loginModelId",
                schema: "dbo",
                table: "Users");
        }
    }
}
