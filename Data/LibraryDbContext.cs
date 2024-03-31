using Microsoft.EntityFrameworkCore;
using Online_Library.Models;
using Online_Library.View_Model.Shared;
using Online_Library.View_Model.Users;

namespace Online_Library.Data
{
    public class LibraryDbContext : DbContext //имплементираме класа DB context който е вграден клас в пакета NuGet Core
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) // опциите които ще се конфигурират тук, ще бъдат  предадени на базовия клас Dbcontext
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<ReaderCard> ReaderCards { get; set; }

        public DbSet<UserReadsBook> UsersReadsBooks { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        public DbSet<UserFavoriteBook> Favorites { get; set; }

        public DbSet<UserBorrowBook> BorrowBooks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            var users = new[]
            {
                new User { Id = 1, Name = "Гергана",
                    Password = "Password12345", Email= "gergankabibli@gmail.com", Username = "gerganka", Role= "Библиотекар", CreatedAt = DateTime.Now},
            };
            var categories = new[]
            {
                new Category { Id = 1, Title =  "Романи", Description = "Категорията на романите включва разнообразни литературни произведения, фокусирани върху развитието на персонални и социални взаимоотношения, често съчетани с интриги и драма." },
                new Category { Id = 2, Title =  "Исторически романи", Description = "Историческите романи предоставят читателят да се потапя във времето и пространството на минали епохи, като комбинират реални исторически събития с фиктивни герои и сюжети.", ParentId = 1},
                new Category { Id = 3, Title =  "Любовни романи", Description = "Любовните романи се фокусират върху развитието на чувствата и връзките между героите. Обичайно съчетават елементи на романтика и драма, предоставяйки на читателите истории за любов и страст.", ParentId = 1},
                new Category { Id = 4, Title =  "Трилър", Description = "Трилърите са жанр, чиито истории са изпълнени с напрежение, интриги и неочаквани обрати. Героите се изправят пред опасности и загадки, които често водят до неочаквани развръзки." },
                new Category { Id = 5, Title =  "Фантастика", Description = "Фантастиката предоставя възможност за изследване на невероятни светове и идеи. Този жанр включва различни подкатегории,които предоставят научнофантастични или фантастични приключения." },
                new Category { Id = 6, Title =  "Научна фантастика", Description = "Научната фантастика се фокусира върху научни и технологични аспекти, представяйки възможни бъдещи сценарии и технологични развития. Често включва елементи на изследователския дух и технологичен прогрес.", ParentId = 5},
                new Category { Id = 7, Title =  "Фентъзи", Description = "Жанрът на фентъзито включва елементи от въображаемо и магическо, предоставяйки на читателите възможността да се потопят в светове, изпълнени с магия, удивителни създания и зашеметяващи приключения.", ParentId = 6},
                new Category { Id = 8, Title =  "Хорър", Description = "Хорърът е жанр, целящ да интригува и плаши читателя. Често включва неестествени явления, ужасяващи събития и напрежение, които създават усещане за страх и тревога." },
                new Category { Id = 9, Title =  "Фолклор", Description = "Категорията на фолклора обхваща традиционни и народни истории, легенди, митове и приказки. Тези произведения отразяват културното богатство и наследство на различни общества." },
                new Category { Id = 10, Title = "Хумор и сатира", Description = "Книгите в тази категория имат за цел да развеселят и забавляват читателя чрез хумористични елементи и сатирични образи. Често използват ирония и смешни ситуации, за да предизвикат смях." },
                new Category { Id = 11, Title = "Детска и Юношеска литература", Description = "Детската и юношеска литература е насочена към младата аудитория и обикновено включва приключения, образователни елементи и развиващи сюжети, които съчетават забавление с поучение." },
                new Category { Id = 12, Title = "Приключенска литература", Description = "Приключенските романи,предоставят възможността читателят да се потопи в изпълнени с действие истории. Героите често се изправят пред различни предизвикателства и опасности." },
                new Category { Id = 13, Title = "Поезия", Description = "Поезията представлява изразително изкуство, което използва ритъм, рима и изразителен език, за да предаде емоции, образи и мисли. Този жанр предоставя разнообразие от стилове и теми." } ,
                new Category { Id = 14, Title = "Изкуство", Description = "Категорията на изкуството обхваща литературни произведения, посветени на различни аспекти на изобразителните, изпълнителските и литературните изкуства." },
                new Category { Id = 15, Title = "Общество", Description = "Категорията на обществото включва литературни произведения, които се занимават с различни аспекти на човешкото общество."}
            };
            modelBuilder.Entity<User>().HasData(users);
            modelBuilder.Entity<Category>().HasData(categories);



            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
           
        }


        public DbSet<Online_Library.View_Model.Users.Register> Register { get; set; } = default!;
        public DbSet<Online_Library.View_Model.Users.LoginVM> LoginVM { get; set; } = default!;
    }
}
