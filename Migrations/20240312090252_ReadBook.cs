using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Library.Migrations
{
    /// <inheritdoc />
    public partial class ReadBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "UsersReadsBooks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 3, 12, 11, 2, 51, 534, DateTimeKind.Local).AddTicks(7415));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "UsersReadsBooks");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 3, 12, 9, 19, 55, 288, DateTimeKind.Local).AddTicks(3597));
        }
    }
}
