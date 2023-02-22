using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Migrations.UserAccount
{
    /// <inheritdoc />
    public partial class UpdatedColumnsAndRows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                schema: "dbo",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "dbo",
                newName: "users",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "UserName",
                schema: "dbo",
                table: "users",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Country",
                schema: "dbo",
                table: "users",
                newName: "country");

            migrationBuilder.RenameColumn(
                name: "City",
                schema: "dbo",
                table: "users",
                newName: "city");

            migrationBuilder.RenameColumn(
                name: "lastName",
                schema: "dbo",
                table: "users",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "UserRole",
                schema: "dbo",
                table: "users",
                newName: "user_role");

            migrationBuilder.RenameColumn(
                name: "RegistrationDate",
                schema: "dbo",
                table: "users",
                newName: "registration_date");

            migrationBuilder.RenameColumn(
                name: "LastActive",
                schema: "dbo",
                table: "users",
                newName: "last_active");

            migrationBuilder.RenameColumn(
                name: "LangPreference",
                schema: "dbo",
                table: "users",
                newName: "lang_preference");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                schema: "dbo",
                table: "users",
                newName: "first_name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                schema: "dbo",
                table: "users",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                schema: "dbo",
                table: "users");

            migrationBuilder.RenameTable(
                name: "users",
                schema: "dbo",
                newName: "Users",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "username",
                schema: "dbo",
                table: "Users",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "country",
                schema: "dbo",
                table: "Users",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "city",
                schema: "dbo",
                table: "Users",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "user_role",
                schema: "dbo",
                table: "Users",
                newName: "UserRole");

            migrationBuilder.RenameColumn(
                name: "registration_date",
                schema: "dbo",
                table: "Users",
                newName: "RegistrationDate");

            migrationBuilder.RenameColumn(
                name: "last_name",
                schema: "dbo",
                table: "Users",
                newName: "lastName");

            migrationBuilder.RenameColumn(
                name: "last_active",
                schema: "dbo",
                table: "Users",
                newName: "LastActive");

            migrationBuilder.RenameColumn(
                name: "lang_preference",
                schema: "dbo",
                table: "Users",
                newName: "LangPreference");

            migrationBuilder.RenameColumn(
                name: "first_name",
                schema: "dbo",
                table: "Users",
                newName: "FirstName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                schema: "dbo",
                table: "Users",
                column: "id");
        }
    }
}
