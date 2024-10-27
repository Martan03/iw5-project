using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IW5Forms.Api.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class final_form_repair : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Users_ResponderId",
                table: "Answers");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Forms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ResponderId",
                table: "Answers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_OwnerId",
                table: "Forms",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Users_ResponderId",
                table: "Answers",
                column: "ResponderId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_Users_OwnerId",
                table: "Forms",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Users_ResponderId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Forms_Users_OwnerId",
                table: "Forms");

            migrationBuilder.DropIndex(
                name: "IX_Forms_OwnerId",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Forms");

            migrationBuilder.AlterColumn<Guid>(
                name: "ResponderId",
                table: "Answers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Users_ResponderId",
                table: "Answers",
                column: "ResponderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
