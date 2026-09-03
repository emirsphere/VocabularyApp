using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vocabulary.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixUserRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLevelProgress_Users_UserId1",
                table: "UserLevelProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMeaningProgress_Users_UserId1",
                table: "UserMeaningProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_UserQuestionProgress_Users_UserId1",
                table: "UserQuestionProgress");

            migrationBuilder.DropIndex(
                name: "IX_UserQuestionProgress_UserId1",
                table: "UserQuestionProgress");

            migrationBuilder.DropIndex(
                name: "IX_UserMeaningProgress_UserId1",
                table: "UserMeaningProgress");

            migrationBuilder.DropIndex(
                name: "IX_UserLevelProgress_UserId1",
                table: "UserLevelProgress");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserQuestionProgress");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserMeaningProgress");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserLevelProgress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "UserQuestionProgress",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "UserMeaningProgress",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "UserLevelProgress",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestionProgress_UserId1",
                table: "UserQuestionProgress",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserMeaningProgress_UserId1",
                table: "UserMeaningProgress",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserLevelProgress_UserId1",
                table: "UserLevelProgress",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLevelProgress_Users_UserId1",
                table: "UserLevelProgress",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMeaningProgress_Users_UserId1",
                table: "UserMeaningProgress",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuestionProgress_Users_UserId1",
                table: "UserQuestionProgress",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
