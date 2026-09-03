using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vocabulary.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "CreatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserQuestionProgress");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserMeaningProgress");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserLevelProgress");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }
    }
}
