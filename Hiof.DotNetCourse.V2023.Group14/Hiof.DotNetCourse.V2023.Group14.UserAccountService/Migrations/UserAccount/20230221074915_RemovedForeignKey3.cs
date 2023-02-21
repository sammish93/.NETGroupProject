using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Migrations.UserAccount
{
    /// <inheritdoc />
    public partial class RemovedForeignKey3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_login_verification_loginModelId",
                schema: "dbo",
                table: "users");

            migrationBuilder.DropTable(
                name: "login_verification",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_users_loginModelId",
                schema: "dbo",
                table: "users");

            migrationBuilder.DropColumn(
                name: "loginModelId",
                schema: "dbo",
                table: "users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "loginModelId",
                schema: "dbo",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "login_verification",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    password = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    salt = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    token = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    username = table.Column<string>(type: "nvarchar(500)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_login_verification", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_loginModelId",
                schema: "dbo",
                table: "users",
                column: "loginModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_users_login_verification_loginModelId",
                schema: "dbo",
                table: "users",
                column: "loginModelId",
                principalSchema: "dbo",
                principalTable: "login_verification",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
