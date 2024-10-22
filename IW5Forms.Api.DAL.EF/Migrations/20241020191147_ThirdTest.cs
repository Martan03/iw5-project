using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IW5Forms.Api.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class ThirdTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Forms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Public",
                table: "Forms",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "Public",
                table: "Forms");
        }
    }
}
