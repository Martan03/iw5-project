using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IW5Forms.Api.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class entity_overhaul : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forms_Users_OwnerId",
                table: "Forms");

            migrationBuilder.DropTable(
                name: "FormEntityUserEntity");

            migrationBuilder.DropIndex(
                name: "IX_Forms_OwnerId",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "Public",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Answers");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Options",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<Guid>(
                name: "ResponderId",
                table: "Answers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "UserForms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormRelationTypes = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserForms_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserForms_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserForms_FormId",
                table: "UserForms",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_UserForms_UserId",
                table: "UserForms",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserForms");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Options",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ResponderId",
                table: "Answers");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Forms",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "Public",
                table: "Forms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Answers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FormEntityUserEntity",
                columns: table => new
                {
                    AvailableFormsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersWithAccessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormEntityUserEntity", x => new { x.AvailableFormsId, x.UsersWithAccessId });
                    table.ForeignKey(
                        name: "FK_FormEntityUserEntity_Forms_AvailableFormsId",
                        column: x => x.AvailableFormsId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormEntityUserEntity_Users_UsersWithAccessId",
                        column: x => x.UsersWithAccessId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Forms_OwnerId",
                table: "Forms",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_FormEntityUserEntity_UsersWithAccessId",
                table: "FormEntityUserEntity",
                column: "UsersWithAccessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_Users_OwnerId",
                table: "Forms",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
