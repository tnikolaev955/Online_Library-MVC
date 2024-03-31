using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_Library.Data;


namespace UnitTestLibrary
{
    internal class DummyDataDbInitializer
    {


        public void Seed(LibraryDbContext context)
        {

            context.User.AddRange(
                 new User
                 {
                     Id = 700,
                     Name = "Test1",
                     Password = "Test12345",
                     Email = "test@gmail.com",
                     Username = "test",
                     Role = "Библиотекар",
                     CreatedAt = DateTime.Now
                 },
                    new User
                    {
                        Id = 701,
                        Name = "Test2",
                        Password = "Test",
                        Email = "test2@gmail.com",
                        Username = "test2",
                        Role = "Читател",
                        CreatedAt = DateTime.Now
                    },



                );

            context.SaveChanges();
        }
    }

}
