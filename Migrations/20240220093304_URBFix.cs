using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Library.Migrations
{
    /// <inheritdoc />
    public partial class URBFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReadsBook_Books_BookId",
                table: "UserReadsBook");

            migrationBuilder.DropForeignKey(
                name: "FK_UserReadsBook_Users_UserId",
                table: "UserReadsBook");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserReadsBook",
                table: "UserReadsBook");

            migrationBuilder.RenameTable(
                name: "UserReadsBook",
                newName: "UsersReadsBooks");

            migrationBuilder.RenameIndex(
                name: "IX_UserReadsBook_UserId",
                table: "UsersReadsBooks",
                newName: "IX_UsersReadsBooks_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserReadsBook_BookId",
                table: "UsersReadsBooks",
                newName: "IX_UsersReadsBooks_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersReadsBooks",
                table: "UsersReadsBooks",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 2, 20, 11, 33, 4, 38, DateTimeKind.Local).AddTicks(3182));

            migrationBuilder.AddForeignKey(
                name: "FK_UsersReadsBooks_Books_BookId",
                table: "UsersReadsBooks",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersReadsBooks_Users_UserId",
                table: "UsersReadsBooks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersReadsBooks_Books_BookId",
                table: "UsersReadsBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersReadsBooks_Users_UserId",
                table: "UsersReadsBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersReadsBooks",
                table: "UsersReadsBooks");

            migrationBuilder.RenameTable(
                name: "UsersReadsBooks",
                newName: "UserReadsBook");

            migrationBuilder.RenameIndex(
                name: "IX_UsersReadsBooks_UserId",
                table: "UserReadsBook",
                newName: "IX_UserReadsBook_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UsersReadsBooks_BookId",
                table: "UserReadsBook",
                newName: "IX_UserReadsBook_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserReadsBook",
                table: "UserReadsBook",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 2, 20, 11, 22, 37, 56, DateTimeKind.Local).AddTicks(4458));

            migrationBuilder.AddForeignKey(
                name: "FK_UserReadsBook_Books_BookId",
                table: "UserReadsBook",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserReadsBook_Users_UserId",
                table: "UserReadsBook",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
