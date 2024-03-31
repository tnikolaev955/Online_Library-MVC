using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Online_Library.Data;
using Online_Library.Models;
using Online_Library.View_Model.Books;
using Online_Library.View_Model.Users;
using System.Diagnostics;

namespace Online_Library.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LibraryDbContext _db;

        public HomeController(ILogger<HomeController> logger, LibraryDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var top5Books = _db.Books
                .Include(b => b.Category)
                .Include(b => b.Ratings)
                .AsEnumerable()
                .OrderByDescending(b => b.AvgRating)
                .Take(6)
                .ToList();
            HomeBooksVM model = new HomeBooksVM();
            model.Top5Books = top5Books;

            var value = HttpContext.Session.GetString("loggedUser");
            User loggedUser = null;
            if (value != null)
            {
                loggedUser = JsonConvert.DeserializeObject<User>(value);
            }

            if (loggedUser == null)
            {
                return View(model);
            }


            List<UserFavoriteBook> userFavoriteBooks = _db.Favorites.Include(f => f.Book).Include(f => f.Book.Category).Where(f => f.UserId == loggedUser.Id).ToList();

            if(userFavoriteBooks.Count <= 0)
            {
                return View(model);
            }

            List<Book> recommendedBooks = new List<Book>();
            List<Book> allBooks = _db.Books.Include(b => b.Category).Include(b => b.Ratings).ToList();
            foreach (var favoriteBook in userFavoriteBooks)
            {
                var fb = favoriteBook.Book;

                foreach (var book in allBooks)
                {
                    if (book.Id != favoriteBook.Id && (book.Author == fb.Author || book.Category.Id == fb.Category.Id) && !recommendedBooks.Any(b=>b.Id==book.Id))
                    {
                        recommendedBooks.Add(book);
                    }
                }

            }
            var sortedRecommendedBooks = recommendedBooks.OrderByDescending(b=>b.AvgRating).Take(6).ToList();
            model.RecommendedBooks = sortedRecommendedBooks;
            return View(model);
        }

        public IActionResult forUs()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Register model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                User registeredUser = new User()
                {

                    CreatedAt = DateTime.Now,
                    Birthdate = model.Birthdate,
                    Email = model.Email,
                    Role = "Читател",
                    Name = model.Name,
                    Password = model.Password,
                    Username = model.Username
                };


                _db.Users.Add(registeredUser);
                _db.SaveChanges();
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginVM model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            User user = _db.Users.SingleOrDefault(u => u.Username == model.Username && u.Password == model.Password);
            if (user == null)
            {
                this.ModelState.AddModelError("AuthError", "Невалидни данни за вход.");
                return View(model);
            }

            string jsonData = JsonConvert.SerializeObject(user);
            HttpContext.Session.SetString("loggedUser", jsonData);

            var borrowedBooksCount = _db.BorrowBooks.Where(b => b.UserId == user.Id && b.DateOfReturn == null).ToList().Count();
            TempData["BorrowedBooksCount"] = borrowedBooksCount;
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("loggedUser");
            return RedirectToAction("Login", "Home");
        }


        public IActionResult Profile()
        {
            var value = HttpContext.Session.GetString("loggedUser");
            User loggedUser = null;
            if (value != null)
            {
                loggedUser = JsonConvert.DeserializeObject<User>(value);
            }

            if (loggedUser == null) // Check if loggedUser is null
            {
                return RedirectToAction(nameof(Index)); // Redirect to Index if not logged in
            }
            return View(loggedUser);
        }
        [HttpGet]
        public ActionResult EditProfile()
        {
            var value = HttpContext.Session.GetString("loggedUser");
            User loggedUser = null;
            if (value != null)
            {
                loggedUser = JsonConvert.DeserializeObject<User>(value);
            }

            if (loggedUser == null) // Check if loggedUser is null
            {
                return RedirectToAction(nameof(Index)); // Redirect to Index if not logged in
            }
            EditProfileVM model = new EditProfileVM
            {
                Name = loggedUser.Name,
                Username = loggedUser.Username,
                Email = loggedUser.Email,
                OldPassword = "",
                Password = "",
                Birthdate = loggedUser.Birthdate
            };
            if (model.Username != null)
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult EditProfile(EditProfileVM model)
        {
            var value = HttpContext.Session.GetString("loggedUser");
            User loggedUser = null;
            if (value != null)
            {
                loggedUser = JsonConvert.DeserializeObject<User>(value);
            }

            if (loggedUser == null) // Check if loggedUser is null
            {
                return RedirectToAction(nameof(Index)); // Redirect to Index if not logged in
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }



                if (loggedUser == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                if (model.OldPassword != loggedUser.Password)
                {
                    ModelState.AddModelError("", "Въведохте грешна стара парола");
                    return View(model);
                }

                loggedUser.Name = model.Name;
                loggedUser.Username = model.Username;
                loggedUser.Birthdate = model.Birthdate;
                loggedUser.UpdatedAt = DateTime.Now;
                loggedUser.Password = model.Password;

                HttpContext.Session.Remove("loggedUser");
                string jsonData = JsonConvert.SerializeObject(loggedUser);
                HttpContext.Session.SetString("loggedUser", jsonData);

                _db.Users.Update(loggedUser);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public IActionResult DeleteLoggedUser()
        {
            var value = HttpContext.Session.GetString("loggedUser");
            User loggedUser = null;
            if (value != null)
            {
                loggedUser = JsonConvert.DeserializeObject<User>(value);
            }

            if (loggedUser == null) // Check if loggedUser is null
            {
                return RedirectToAction(nameof(Index)); // Redirect to Index if not logged in
            }

            HttpContext.Session.Remove("loggedUser");
            _db.Users.Remove(loggedUser);
            _db.SaveChanges();
            return RedirectToAction("Login", "Home");
        }

        public IActionResult Subscribe()
        {
            var value = HttpContext.Session.GetString("loggedUser");
            User loggedUser = null;
            if (value != null)
            {
                loggedUser = JsonConvert.DeserializeObject<User>(value);
            }

            if (loggedUser == null) // Check if loggedUser is null
            {
                return RedirectToAction(nameof(Index)); // Redirect to Index if not logged in
            }

            return RedirectToAction("Profile", "Home");
        }
    }
}