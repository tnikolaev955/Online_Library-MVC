using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Library.Migrations
{
    /// <inheritdoc />
    public partial class CatalogModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 3, 1, 15, 15, 54, 711, DateTimeKind.Local).AddTicks(1686));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 2, 29, 10, 32, 55, 12, DateTimeKind.Local).AddTicks(2000));
        }
    }
}
