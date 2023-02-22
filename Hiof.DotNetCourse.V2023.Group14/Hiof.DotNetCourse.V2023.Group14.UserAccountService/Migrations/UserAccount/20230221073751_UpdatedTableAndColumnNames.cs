using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Migrations.UserAccount
{
    /// <inheritdoc />
    public partial class UpdatedTableAndColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_LoginVerification_loginModelId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoginVerification",
                schema: "dbo",
                table: "LoginVerification");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "dbo",
                newName: "users",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "LoginVerification",
                schema: "dbo",
                newName: "login_verification",
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

            migrationBuilder.RenameIndex(
                name: "IX_Users_loginModelId",
                schema: "dbo",
                table: "users",
                newName: "IX_users_loginModelId");

            migrationBuilder.RenameColumn(
                name: "UserName",
                schema: "dbo",
                table: "login_verification",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Token",
                schema: "dbo",
                table: "login_verification",
                newName: "token");

            migrationBuilder.RenameColumn(
                name: "Salt",
                schema: "dbo",
                table: "login_verification",
                newName: "salt");

            migrationBuilder.RenameColumn(
                name: "Password",
                schema: "dbo",
                table: "login_verification",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "dbo",
                table: "login_verification",
                newName: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                schema: "dbo",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_login_verification",
                schema: "dbo",
                table: "login_verification",
                column: "id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_login_verification_loginModelId",
                schema: "dbo",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                schema: "dbo",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_login_verification",
                schema: "dbo",
                table: "login_verification");

            migrationBuilder.RenameTable(
                name: "users",
                schema: "dbo",
                newName: "Users",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "login_verification",
                schema: "dbo",
                newName: "LoginVerification",
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

            migrationBuilder.RenameIndex(
                name: "IX_users_loginModelId",
                schema: "dbo",
                table: "Users",
                newName: "IX_Users_loginModelId");

            migrationBuilder.RenameColumn(
                name: "username",
                schema: "dbo",
                table: "LoginVerification",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "token",
                schema: "dbo",
                table: "LoginVerification",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "salt",
                schema: "dbo",
                table: "LoginVerification",
                newName: "Salt");

            migrationBuilder.RenameColumn(
                name: "password",
                schema: "dbo",
                table: "LoginVerification",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "dbo",
                table: "LoginVerification",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                schema: "dbo",
                table: "Users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoginVerification",
                schema: "dbo",
                table: "LoginVerification",
                column: "Id");

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
    }
}
