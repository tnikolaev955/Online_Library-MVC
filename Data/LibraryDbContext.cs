using Microsoft.EntityFrameworkCore;
using Online_Library.Models;

namespace Online_Library.Data
{
    public class LibraryDbContext : DbContext //имплементираме класа DB context който е вграден клас в пакета NuGet Core
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) // опциите които ще се конфигурират тук, ще бъдат  предадени на базовия клас Dbcontext
        {

        }

        public DbSet<Category> Categories { get; set; }  // името на таблицата в Sql Server  и ще бъде създадена в базата данни
    }
}
