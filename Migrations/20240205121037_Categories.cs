using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Online_Library.Migrations
{
    /// <inheritdoc />
    public partial class Categories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "Description",
                value: "Фантастиката предоставя възможност за изследване на невероятни светове и идеи. Този жанр включва различни подкатегории,които предоставят научнофантастични или фантастични приключения.");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "Description",
                value: "Жанрът на фентъзито включва елементи от въображаемо и магическо, предоставяйки на читателите възможността да се потопят в светове, изпълнени с магия, удивителни създания и зашеметяващи приключения.");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11,
                column: "Title",
                value: "Детска и Юношеска литература");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 12,
                column: "Description",
                value: "Приключенските романи,предоставят възможността читателят да се потопи в изпълнени с действие истории. Героите често се изправят пред различни предизвикателства и опасности.");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "ParentId", "Title" },
                values: new object[,]
                {
                    { 14, "Категорията на изкуството обхваща литературни произведения, посветени на различни аспекти на изобразителните, изпълнителските и литературните изкуства.", null, "Изкуство" },
                    { 15, "Категорията на обществото включва литературни произведения, които се занимават с различни аспекти на човешкото общество.", null, "Общество" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 2, 5, 14, 10, 36, 485, DateTimeKind.Local).AddTicks(3933));

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_BookId",
                table: "Ratings",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId",
                table: "Ratings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Books_BookId",
                table: "Ratings",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Users_UserId",
                table: "Ratings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Books_BookId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Users_UserId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_BookId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserId",
                table: "Ratings");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "Description",
                value: "Фантастиката предоставя възможност за изследване на невероятни светове и идеи. Този жанр включва различни подкатегории, които предоставят читателите научнофантастични или фантастични приключения.");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "Description",
                value: "Фентъзито включва елементи от въображаемо и магическо, предоставяйки на читателите възможността да избягат от реалността и да се потопят в светове, пълни с магия, създания и приключения.");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11,
                column: "Title",
                value: "Детска и Юношеска Лит.");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 12,
                column: "Description",
                value: "Приключенските романи предоставят читателят възможността да се потопят в изпълнени с действие истории. Героите често се изправят пред различни предизвикателства и опасности.");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 2, 2, 14, 44, 23, 628, DateTimeKind.Local).AddTicks(4063));
        }
    }
}
