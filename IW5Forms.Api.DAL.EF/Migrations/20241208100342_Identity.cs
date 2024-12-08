using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IW5Forms.Api.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class Identity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityOwnerId",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityOwnerId",
                table: "UserForms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityOwnerId",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityOwnerId",
                table: "Forms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityOwnerId",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityOwnerId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IdentityOwnerId",
                table: "UserForms");

            migrationBuilder.DropColumn(
                name: "IdentityOwnerId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IdentityOwnerId",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "IdentityOwnerId",
                table: "Answers");
        }
    }
}
