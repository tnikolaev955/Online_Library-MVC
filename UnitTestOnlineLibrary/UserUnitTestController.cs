using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Library.Controllers;
using Online_Library.Data;
using Online_Library.Models;
using System.ComponentModel.DataAnnotations;

namespace UnitTestOnlineLibrary
{
    [TestClass]
    public class UserUnitTestController
    {
        private static DbContextOptions<LibraryDbContext> dbContextOptions { get; }
        public static string connectionString = "Server=DESKTOP-V61UN36;Database=LibraryDb;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True";
        static UserUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<LibraryDbContext>().UseSqlServer(connectionString).Options;

            var context = new LibraryDbContext(dbContextOptions);

        }

        [TestMethod]
        public void Index_ReturnsAllUsers()
        {
            using (var context = new LibraryDbContext(dbContextOptions))
            {
                var controller = new UserController(context);
                var result = controller.Index();
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(ViewResult));
                var viewResult = (ViewResult)result;
                var model = (IEnumerable<User>)viewResult.Model;
                Assert.IsTrue(model.Any());
            }
        }

        [TestMethod]
        public void Name_requiredAttribute_Exists()
        {
            //Arrange
            var propertyIndfo = typeof(User).GetProperty("Name");

            //Act
            var requiredAttribute = (RequiredAttribute)Attribute.GetCustomAttribute(propertyIndfo, typeof(RequiredAttribute));


            //Assert
            Assert.IsNotNull(@requiredAttribute);
        }


        [TestMethod]
        public void DetailsReturnsViewResult_WithUser()
        {
            using (var context = new LibraryDbContext(dbContextOptions))
            {
                var user = new User
                {
                    Name = "Test",
                    Username = "Test_Username",
                    Email = "testEmail@gmail.com",
                    Password = "Password123",
                    Birthdate = DateOnly.FromDateTime(DateTime.Now.AddDays(-19)),
                    Role = "Читател",
                };
                context.Users.Add(user);
                context.SaveChanges();
                var controller = new UserController(context);
                var result = controller.Details(user.Id);

                Assert.IsInstanceOfType(result, typeof(ViewResult));
                var viewResult = (ViewResult)result;
                var model = (User)viewResult.Model;
                Assert.IsNotNull(model);
                Assert.AreEqual(user.Id, model.Id);
            }
        }

        [TestMethod]
        public void Create_withValidModel_RedirectsToIndex()
        {
            using (var context = new LibraryDbContext(dbContextOptions))
            {
                var controller = new UserController(context);
                var model = new User
                {
                    Name = "TestUser",
                    Username = "Test_Username",
                    Email = "testEmail@gmail.com",
                    Password = "Password123",
                    Birthdate = DateOnly.FromDateTime(DateTime.Now.AddDays(-19)),
                    Role = "Читател"
                 
                };

                var result = controller.Create(model);
                Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
                var redirectToAction = (RedirectToActionResult)result;
                Assert.AreEqual("Index", redirectToAction.ActionName);
            }
        }

        [TestMethod]
        public void Create_WithInvalidModel_ReturnViewResult_WithModel()
        {
            using (var context = new LibraryDbContext(dbContextOptions))
            {
                var controller = new UserController(context);
                var model = new User
                {
                    Name = "TestUser",
                    Username = "Test_Username",
                    Password = "Password123"

                };

                controller.ModelState.AddModelError("Email", " Имейлът е задължително поле");

                var result = controller.Create(model);
                Assert.IsInstanceOfType(result, typeof(ViewResult));
                var viewResult = (ViewResult)result;
                Assert.AreEqual(model, viewResult.Model);
            }
        }
    }
}