using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IW5Forms.Api.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class devet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Answers_ResponderId",
                table: "Answers",
                column: "ResponderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Users_ResponderId",
                table: "Answers",
                column: "ResponderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Users_ResponderId",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_ResponderId",
                table: "Answers");
        }
    }
}
