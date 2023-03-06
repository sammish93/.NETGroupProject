using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Migrations.UserAccount
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
                    id = table.Column<string>(type: "char(36)", nullable: false),
                    username = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    salt = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    token = table.Column<string>(type: "nvarchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_login_verification", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(36)", nullable: false),
                    username = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    email = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    country = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    city = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    lang_preference = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    user_role = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    registration_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_active = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "login_verification",
                columns: new[] { "id", "password", "salt", "token", "username" },
                values: new object[,]
                {
                    { "3fa85f64-5717-4562-b3fc-2c963f66afa6", "70A1AF97C0496AD874DC", "247432D4ED93DCE32929", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIzZmE4NWY2NC01NzE3LTQ1NjItYjNmYy0yYzk2M2Y2NmFmYTYiLCJuYmYiOjE2NzcyMzUxMDEsImV4cCI6MTY3NzQ5NDMwMSwiaWF0IjoxNjc3MjM1MTAxfQ.LqUQyhrnWwkNtkQUYcatydTdeAqvCaZZ4tEYovAGkJI", "JinkxMonsoon" },
                    { "54af86bf-346a-4cba-b36f-527748e1cb93", "A7E220F0781BE0C248A3", "3E921C45F3A9089BDC7E", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI1NGFmODZiZi0zNDZhLTRjYmEtYjM2Zi01Mjc3NDhlMWNiOTMiLCJuYmYiOjE2NzcyMzU0OTQsImV4cCI6MTY3NzQ5NDY5NCwiaWF0IjoxNjc3MjM1NDk0fQ._VGOcvMVXmtj741AoUGLYnWsAvG5geuLHX_phvfOuT8", "testaccount" },
                    { "e8cc12ba-4df6-4b06-b96e-9ad00a927a93", "B1A8A1223DCA3A102726", "A91F72A37D0E46037B85", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJlOGNjMTJiYS00ZGY2LTRiMDYtYjk2ZS05YWQwMGE5MjdhOTMiLCJuYmYiOjE2NzcyMzUyNDcsImV4cCI6MTY3NzQ5NDQ0NywiaWF0IjoxNjc3MjM1MjQ3fQ.NMCEXx8Dhr40krHQrpz4Zwgslj9N_HN3fi_Qrt4oMes", "QueenOfTheNorth" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "users",
                columns: new[] { "id", "city", "country", "email", "first_name", "lang_preference", "last_active", "last_name", "password", "registration_date", "user_role", "username" },
                values: new object[,]
                {
                    { "3fa85f64-5717-4562-b3fc-2c963f66afa6", "Seattle", "USA", "joojoo@gmail.com", "Jinkx", "en", new DateTime(2023, 2, 24, 10, 37, 8, 387, DateTimeKind.Unspecified), "Monsoon", "70A1AF97C0496AD874DC", new DateTime(2023, 2, 24, 10, 37, 8, 387, DateTimeKind.Unspecified), "User", "JinkxMonsoon" },
                    { "54af86bf-346a-4cba-b36f-527748e1cb93", "Oslo", "Norway", "testme@test.no", "Ola", "no", new DateTime(2023, 2, 24, 10, 42, 49, 373, DateTimeKind.Unspecified), "Nordmann", "A7E220F0781BE0C248A3", new DateTime(2023, 2, 24, 10, 42, 49, 373, DateTimeKind.Unspecified), "Admin", "testaccount" },
                    { "e8cc12ba-4df6-4b06-b96e-9ad00a927a93", "Toronto", "Canada", "b_hyteso@gmail.com", "Brooklyn", "en", new DateTime(2023, 2, 24, 10, 39, 32, 540, DateTimeKind.Unspecified), "Hytes", "B1A8A1223DCA3A102726", new DateTime(2023, 2, 24, 10, 39, 32, 540, DateTimeKind.Unspecified), "User", "QueenOfTheNorth" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "login_verification",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "users",
                schema: "dbo");
        }
    }
}
