using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Online_Library.Migrations
{
    /// <inheritdoc />
    public partial class addcategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "ParentId", "Title" },
                values: new object[,]
                {
                    { 1, "Категорията на романите включва разнообразни литературни произведения, фокусирани върху развитието на персонални и социални взаимоотношения, често съчетани с интриги и драма.", null, "Романи" },
                    { 4, "Трилърите са жанр, чиито истории са изпълнени с напрежение, интриги и неочаквани обрати. Героите се изправят пред опасности и загадки, които често водят до неочаквани развръзки.", null, "Трилър" },
                    { 5, "Фантастиката предоставя възможност за изследване на невероятни светове и идеи. Този жанр включва различни подкатегории, които предоставят читателите научнофантастични или фантастични приключения.", null, "Фантастика" },
                    { 8, "Хорърът е жанр, целящ да интригува и плаши читателя. Често включва неестествени явления, ужасяващи събития и напрежение, които създават усещане за страх и тревога.", null, "Хорър" },
                    { 9, "Категорията на фолклора обхваща традиционни и народни истории, легенди, митове и приказки. Тези произведения отразяват културното богатство и наследство на различни общества.", null, "Фолклор" },
                    { 10, "Книгите в тази категория имат за цел да развеселят и забавляват читателя чрез хумористични елементи и сатирични образи. Често използват ирония и смешни ситуации, за да предизвикат смях.", null, "Хумор и сатира" },
                    { 11, "Детската и юношеска литература е насочена към младата аудитория и обикновено включва приключения, образователни елементи и развиващи сюжети, които съчетават забавление с поучение.", null, "Детска и Юношеска Лит." },
                    { 12, "Приключенските романи предоставят читателят възможността да се потопят в изпълнени с действие истории. Героите често се изправят пред различни предизвикателства и опасности.", null, "Приключенска литература" },
                    { 13, "Поезията представлява изразително изкуство, което използва ритъм, рима и изразителен език, за да предаде емоции, образи и мисли. Този жанр предоставя разнообразие от стилове и теми.", null, "Поезия" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2024, 2, 2, 14, 34, 8, 124, DateTimeKind.Local).AddTicks(670), "Гергана" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "ParentId", "Title" },
                values: new object[,]
                {
                    { 2, "Историческите романи предоставят читателят да се потапя във времето и пространството на минали епохи, като комбинират реални исторически събития с фиктивни герои и сюжети.", 1, "Исторически романи" },
                    { 3, "Любовните романи се фокусират върху развитието на чувствата и връзките между героите. Обичайно съчетават елементи на романтика и драма, предоставяйки на читателите истории за любов и страст.", 1, "Любовни романи" },
                    { 6, "Научната фантастика се фокусира върху научни и технологични аспекти, представяйки възможни бъдещи сценарии и технологични развития. Често включва елементи на изследователския дух и технологичен прогрес.", 5, "Научна фантастика" },
                    { 7, "Фентъзито включва елементи от въображаемо и магическо, предоставяйки на читателите възможността да избягат от реалността и да се потопят в светове, пълни с магия, създания и приключения.", 6, "Фентъзи" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2024, 1, 24, 14, 47, 30, 684, DateTimeKind.Local).AddTicks(4167), "Иванка" });
        }
    }
}
