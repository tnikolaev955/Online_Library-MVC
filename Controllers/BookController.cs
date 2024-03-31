using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Online_Library.Data;
using Online_Library.Filters;
using Online_Library.Models;
using Online_Library.View_Model;
using System.Text.Json;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.Text.Json.Serialization;
using Online_Library.View_Model.Shared;
using Online_Library.View_Model.Books;



namespace Online_Library.Controllers
{
    public class BookController : Controller
    {
        private readonly LibraryDbContext _db;

        //Връща пътя до wwwroot папката където ще записваме нашите pdf filove
        private readonly IHostingEnvironment _environment;
        private readonly string fileName;
        private string PictureLink;

        public BookController(LibraryDbContext db, IHostingEnvironment hostingEnvironment)
        {
            _db = db;
            _environment = hostingEnvironment;
        }


        [FilterAttribute]
        public ActionResult Index()
        {
            List<Book> books = _db.Books.Include(b => b.Category).ToList();
            Console.WriteLine("Books count: " + books.Count);
            return View(books);
        }

        public ActionResult Catalog(CatalogModel model)
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            var seriazedData = TempData["Books"] as string;

            if (!string.IsNullOrEmpty(seriazedData))
            {
                List<Book> books = System.Text.Json.JsonSerializer.Deserialize<List<Book>>(seriazedData, options);
                model.books = books;
            }
            else
            {
                var books = _db.Books.Include(b => b.Ratings).ToList();

                if (!string.IsNullOrEmpty(model.SortBy))
                {
                    switch (model.SortBy)
                    {
                        case "year":
                            books = model.SortOrder == "desc" ? books.OrderByDescending(b => b.YearOfPublish).ToList() : books.OrderBy(b => b.YearOfPublish).ToList();
                            break;
                        case "A-Z":
                            books = model.SortOrder == "desc" ? books.OrderByDescending(b => b.Title).ToList() : books.OrderBy(b => b.Title).ToList();
                            break;
                        case "rating":
                            books = model.SortOrder == "desc" ? books.OrderByDescending(b => b.AvgRating).ToList() : books.OrderBy(b => b.AvgRating).ToList();
                            break;
                        default:
                            break;

                    }
                }

                model.Pager = model.Pager ?? new PagerVM();
                model.Pager.Page = model.Pager.Page <= 0 ? 1 : model.Pager.Page;
                model.Pager.ItemsPerPage = model.Pager.ItemsPerPage <= 0 ? 10 : model.Pager.ItemsPerPage;

                int itemsPerPage = model.Pager.ItemsPerPage;
                int page = model.Pager.Page;

                model.Pager.PagesCounter = (int)Math.Ceiling(books.Count() / (double)model.Pager.ItemsPerPage);


                model.books = books
                    .Skip(itemsPerPage * (page - 1))
                    .Take(itemsPerPage)
                    .ToList();


            }




            model.categories = _db.Categories.ToList();

            return View(model);

        }



        [FilterAttribute]
        public ActionResult Details(int id)
        {

            var value = HttpContext.Session.GetString("loggedUser");
            User loggedUser = null;
            if (value != null)
            {
                loggedUser = JsonConvert.DeserializeObject<User>(value);
            }

            if (loggedUser == null) // Check if loggedUser is null
            {
                return RedirectToAction(nameof(Index));
            }



            Book book = _db.Books.Include(b => b.Category).Include(b => b.UsersWhoRead).SingleOrDefault(b => b.Id == id);
            BookDetailsVM bookDetailsVM = new BookDetailsVM();
            Rating rating = new Rating();

            bookDetailsVM.Readers = new List<User>();
            if (book.UsersWhoRead != null && book.UsersWhoRead.Count > 0)
            {
                var sortedUser = book.UsersWhoRead.OrderByDescending(b => b.Id).Take(10);
                foreach (UserReadsBook urb in sortedUser)
                {
                    if (urb.IsRead)
                        bookDetailsVM.Readers.Add(_db.Users.FirstOrDefault(u => u.Id == urb.UserId));
                }
            }

            bookDetailsVM.Ratings = _db.Ratings.Where(r => r.BookId == id).Include(r => r.User).ToList();
            bookDetailsVM.Book = book;
            bookDetailsVM.Rating = rating;
            bookDetailsVM.inFavorite = _db.Favorites.Any(ur => ur.UserId == loggedUser.Id && ur.BookId == book.Id);
            bookDetailsVM.isAlreadyBorrowed = _db.BorrowBooks.Any(bb => bb.UserId == loggedUser.Id && bb.BookId == book.Id && bb.DateOfReturn == null);
            bookDetailsVM.userPage = _db.UsersReadsBooks.Any(ur => ur.UserId == loggedUser.Id && ur.BookId == book.Id)
                ? _db.UsersReadsBooks.FirstOrDefault(ur => ur.UserId == loggedUser.Id && ur.BookId == book.Id).Page : 0;
            if (bookDetailsVM != null)
            {
                return View(bookDetailsVM);
            }
            return RedirectToAction("Index");

        }


        public IActionResult BorrowBook(int id)
        {
            var value = HttpContext.Session.GetString("loggedUser");
            User loggedUser = null;
            if (value != null)
            {
                loggedUser = JsonConvert.DeserializeObject<User>(value);
            }

            if (loggedUser == null) // Check if loggedUser is null
            {
                return RedirectToAction(nameof(Index));
            }
            Book book = _db.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return RedirectToAction("Index", "Book");

            }

            UserBorrowBook userBorrow = new UserBorrowBook
            {
                DateOfBorrow = DateTime.Now,
                UserId = loggedUser.Id,
                BookId = book.Id,
            };

            _db.BorrowBooks.Add(userBorrow);
            _db.SaveChanges();

            return RedirectToAction(nameof(BorrowedBooks), "Book");
        }

        public IActionResult ReturnBook(int id)
        {
            var value = HttpContext.Session.GetString("loggedUser");
            User loggedUser = null;
            if (value != null)
            {
                loggedUser = JsonConvert.DeserializeObject<User>(value);
            }

            if (loggedUser == null) // Check if loggedUser is null
            {
                return RedirectToAction(nameof(Index), "Book");
            }

            UserBorrowBook userBorrow = _db.BorrowBooks.FirstOrDefault(bb => bb.Id == id);

            if (userBorrow != null)
            {
                userBorrow.DateOfReturn = DateTime.Now;
                _db.BorrowBooks.Update(userBorrow);
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(BorrowedBooks), "Book");
        }

        public IActionResult BorrowedBooks()
        {
            var value = HttpContext.Session.GetString("loggedUser");
            User loggedUser = null;
            if (value != null)
            {
                loggedUser = JsonConvert.DeserializeObject<User>(value);
            }

            if (loggedUser == null) // Check if loggedUser is null
            {
                return RedirectToAction(nameof(Index));
            }

            List<UserBorrowBook> borrow = _db.BorrowBooks.Include(uf => uf.Book).Where(ur => ur.UserId == loggedUser.Id && ur.DateOfReturn == null).ToList();
            List<UserBorrowBook> returned = _db.BorrowBooks.Include(uf => uf.Book).Where(ur => ur.UserId == loggedUser.Id && ur.DateOfReturn != null).ToList();

            BorrowVM borrowVM = new BorrowVM();
            borrowVM.toReturn = borrow;
            borrowVM.returned = returned;

            return View(borrowVM);
        }

        [FilterAttribute]
        [RoleFilter]
        [HttpGet]
        public ActionResult Create()
        {
            Console.WriteLine("Book Create action");
            List<Category> categories = _db.Categories.ToList();
            ViewBag.Categories = categories.Select(e => new SelectListItem(e.Title, e.Id.ToString()));
            return View();
        }

        [FilterAttribute]
        [RoleFilter]
        [HttpPost]
        public async Task<ActionResult> Create(UploadFileVM model)
        {
            Console.WriteLine("Book Create action httppost is valid:" + ModelState.IsValid);
            List<Category> categories = _db.Categories.ToList();

            ViewBag.Categories = categories.Select(e => new SelectListItem(e.Title, e.Id.ToString()));


            if (model.Book.CategoryId > 0)
            {
                Category category = _db.Categories.Find(model.Book.CategoryId);

                if (category != null)
                {
                    Console.WriteLine("Set category:" + category.Id);
                    model.Book.Category = category;
                    model.Book.CategoryId = category.Id;

                    if (TryValidateModel(model))
                    {
                        Console.WriteLine("Model is valid after adding category.");
                    }
                }
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    if (error.ErrorMessage.ToLower().Contains("категория"))
                    {
                        continue;
                    }
                    else
                    {
                        List<Category> c = _db.Categories.ToList();
                        ViewBag.Categories = c.Select(e => new SelectListItem(e.Title, e.Id.ToString()));
                        return View(model);
                    }
                }

            }

            if (model.File == null)
            {
                List<Category> c = _db.Categories.ToList();
                ViewBag.Categories = c.Select(e => new SelectListItem(e.Title, e.Id.ToString()));
                return View(model);
            }

            string ext = Path.GetExtension(model.File.FileName);
            var fileName = $"model.FileName_{DateTime.Now:ddMMyyyymms}{ext}";


            var filePath = Path.Combine(_environment.WebRootPath, "Img", fileName);

            using (var fileSteam = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(fileSteam);

            }

            Book book = new Book
            {
                Title = model.Book.Title,
                Id = model.Book.Id,
                Author = model.Book.Author,
                Description = model.Book.Description,
                ISBN = model.Book.ISBN,
                CategoryId = model.Book.CategoryId,
                pages = model.Book.pages,
                YearOfPublish = model.Book.YearOfPublish,
                PictureLink = "/Img/" + fileName

            };
            _db.Books.Add(book);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [FilterAttribute]
        [RoleFilter]
        [HttpGet]

        public ActionResult Edit(int id)
        {
            Book book = _db.Books.Find(id);
            UploadFileVM model = new UploadFileVM();
            model.Book = book;

            if (model != null)
            {
                List<Category> categories = _db.Categories.ToList();
                ViewBag.Categories = categories.Select(e => new SelectListItem(e.Title, e.Id.ToString()));
                return View(model);
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        [RoleFilter]
        [FilterAttribute]
        public async Task<ActionResult> Edit(int id, UploadFileVM model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    if (error.ErrorMessage.ToLower().Contains("категория"))
                    {
                        continue;
                    }
                    else
                    {
                        List<Category> c = _db.Categories.ToList();
                        ViewBag.Categories = c.Select(e => new SelectListItem(e.Title, e.Id.ToString()));
                        return View(model);
                    }
                }

            }

            if (model.File == null)
            {
                List<Category> c = _db.Categories.ToList();
                ViewBag.Categories = c.Select(e => new SelectListItem(e.Title, e.Id.ToString()));
                return View(model);
            }

            string ext = Path.GetExtension(model.File.FileName);
            var fileName = $"model.FileName_{DateTime.Now:ddMMyyyymms}{ext}";


            var filePath = Path.Combine(_environment.WebRootPath, "Img", fileName);

            using (var fileSteam = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(fileSteam);

            }



            Book book = _db.Books.Find(id);
            if (book != null)
            {
                book.Title = model.Book.Title;
                book.Author = model.Book.Author;
                book.ISBN = model.Book.ISBN;
                book.Category = model.Book.Category;
                book.pages = model.Book.pages;
                book.YearOfPublish = model.Book.YearOfPublish;
                book.Description = model.Book.Description;
                book.PictureLink = "/Img/" + fileName;

                _db.Books.Update(book);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index");
        }

        [FilterAttribute]
        [RoleFilter]
        public ActionResult Delete(int id)
        {
            Book book = _db.Books.Find(id);
            if (book == null)
            {
                return RedirectToAction(nameof(Index));
            }
            _db.Books.Remove(book);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [FilterAttribute]
        [RoleFilter]
        public ActionResult UploadPdf(int id)
        {
            Book book = _db.Books.Include(b => b.Category).SingleOrDefault(b => b.Id == id);
            UploadFileVM uploadFileVM = new UploadFileVM();
            uploadFileVM.Book = book;
            if (book != null)
            {
                return View(uploadFileVM);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [FilterAttribute]
        [RoleFilter]
        public async Task<ActionResult> UploadPdf(int id, UploadFileVM model)
        {

            if (model.File == null)
            {
                return View(model);
            }

            string ext = Path.GetExtension(model.File.FileName);
            var fileName = $"model.FileName_{DateTime.Now:ddMMyyyymms}{ext}";


            if (ext.ToLower() != ".pdf")
            {
                return View(model);
            }

            var filePath = Path.Combine(_environment.WebRootPath, "Pdfs", fileName);

            using (var fileSteam = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(fileSteam);

            }
            Book book = _db.Books.Include(b => b.Category).SingleOrDefault(b => b.Id == id);
            book.PdfFilePath = "/pdfs/" + fileName;
            _db.Books.Update(book);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [FilterAttribute]
        public IActionResult ReadBook(int id)
        {
            var value = HttpContext.Session.GetString("loggedUser");
            User loggedUser = null;
            if (value != null)
            {
                loggedUser = JsonConvert.DeserializeObject<User>(value);
            }

            if (loggedUser == null) // Check if loggedUser is null
            {
                return RedirectToAction(nameof(Index));
            }
            Book book = _db.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return RedirectToAction(nameof(Index));

            }

            UserReadsBook readsBook = _db.UsersReadsBooks.FirstOrDefault(ur => ur.UserId == loggedUser.Id && ur.BookId == book.Id);

            if (readsBook == null)
            {
                readsBook = new UserReadsBook();
                readsBook.UserId = loggedUser.Id;
                readsBook.BookId = book.Id;
                readsBook.IsRead = false;

                _db.UsersReadsBooks.Add(readsBook);
                _db.SaveChanges();

            }
            return View(readsBook);
        }

        [HttpPost]
        [FilterAttribute]
        public IActionResult ReadBook(UserReadsBook model)
        {

            UserReadsBook userReadsBook = _db.UsersReadsBooks.Include(b => b.Book).Include(u => u.User).FirstOrDefault(urb => urb.Id == model.Id);
            if (model.IsRead == true)
            {
                userReadsBook.IsRead = true;
                userReadsBook.Page = model.Page;
                userReadsBook.DateOfRead = DateTime.Now;
                _db.UsersReadsBooks.Update(userReadsBook);
                _db.SaveChanges();
            }
            else
            {
                userReadsBook.IsRead = false;
                userReadsBook.DateOfRead = null;
                userReadsBook.Page = model.Page;
                _db.UsersReadsBooks.Update(userReadsBook);
                _db.SaveChanges();
            }
            return View(userReadsBook);
        }


        [HttpGet]
        [FilterAttribute]
        public IActionResult MyReadBooks()
        {
            var value = HttpContext.Session.GetString("loggedUser");
            User loggedUser = null;
            if (value != null)
            {
                loggedUser = JsonConvert.DeserializeObject<User>(value);
            }

            if (loggedUser == null) // Check if loggedUser is null
            {
                return RedirectToAction(nameof(Index));
            }


            List<UserReadsBook> readsBook = _db.UsersReadsBooks.Include(ur => ur.Book).Where(ur => ur.UserId == loggedUser.Id && ur.IsRead == true).ToList();

            return View(readsBook);
        }

        [HttpGet]
        [FilterAttribute]
        public IActionResult TopReaders()
        {
            var value = HttpContext.Session.GetString("loggedUser");
            User loggedUser = null;
            if (value != null)
            {
                loggedUser = JsonConvert.DeserializeObject<User>(value);
            }

            if (loggedUser == null) // Check if loggedUser is null
            {
                return RedirectToAction(nameof(Index));
            }

            DateTime lastMonthStart = DateTime.Now.AddMonths(-1);
            List<TopReader> topReaders = _db.UsersReadsBooks
                .Include(ur => ur.Book)
                .Include(ur => ur.User)
                .Where(ur => ur.IsRead == true && ur.DateOfRead >= lastMonthStart && ur.DateOfRead <= DateTime.Now)
                .GroupBy(ur => ur.UserId)
                .Select(ur => new TopReader
                {
                    User = ur.FirstOrDefault().User,
                    bookRead = ur.Count()
                })
                .OrderByDescending(tr => tr.bookRead)
                .Take(10)
                .ToList();

            return View(topReaders);
        }

        public IActionResult AddToFavorites(int id)
        {
            var value = HttpContext.Session.GetString("loggedUser");
            User loggedUser = null;
            if (value != null)
            {
                loggedUser = JsonConvert.DeserializeObject<User>(value);
            }

            if (loggedUser == null) // Check if loggedUser is null
            {
                return RedirectToAction(nameof(Index));
            }
            Book book = _db.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return RedirectToAction(nameof(Index));

            }

            bool alreadyInFavoirtes = _db.Favorites.Any(ur => ur.UserId == loggedUser.Id && ur.BookId == book.Id);

            if (!alreadyInFavoirtes)
            {
                UserFavoriteBook userFavorite = new UserFavoriteBook
                {
                    UserId = loggedUser.Id,
                    BookId = book.Id,
                };

                _db.Favorites.Add(userFavorite);
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(Details), new { id = id });
        }

        public IActionResult RemoveFromFavorites(int id)
        {
            var value = HttpContext.Session.GetString("loggedUser");
            User loggedUser = null;
            if (value != null)
            {
                loggedUser = JsonConvert.DeserializeObject<User>(value);
            }

            if (loggedUser == null) // Check if loggedUser is null
            {
                return RedirectToAction(nameof(Index));
            }
            Book book = _db.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return RedirectToAction(nameof(Index));

            }

            UserFavoriteBook userFavorite = _db.Favorites.FirstOrDefault(ur => ur.UserId == loggedUser.Id && ur.BookId == book.Id);

            if (userFavorite != null)
            {

                _db.Favorites.Remove(userFavorite);
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(Details), new { id = id });
        }

        public IActionResult FavoriteBooks()
        {
            var value = HttpContext.Session.GetString("loggedUser");
            User loggedUser = null;
            if (value != null)
            {
                loggedUser = JsonConvert.DeserializeObject<User>(value);
            }

            if (loggedUser == null) // Check if loggedUser is null
            {
                return RedirectToAction(nameof(Index));
            }

            List<UserFavoriteBook> favorites = _db.Favorites.Include(uf => uf.Book).Where(ur => ur.UserId == loggedUser.Id).ToList();
            List<Book> books = new List<Book>();
            foreach (var favorite in favorites)
            {
                books.Add(favorite.Book);
            }

            return View(books);
        }



        [HttpPost]
        [FilterAttribute]
        public IActionResult RateBook(BookDetailsVM model)
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

            if (!ModelState.IsValid)
            {
                if (model.Book == null || model.Book.Id == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Details), new { id = model.Book.Id });
            }

            Rating rating = new Rating();
            rating.Comment = model.Rating.Comment;
            rating.Rate = model.Rating.Rate;
            rating.UserId = loggedUser.Id;
            //rating.User = loggedUser;
            rating.BookId = model.Rating.BookId;
            // rating.Book = _db.Books.FirstOrDefault(b => b.Id == model.Rating.BookId);

            _db.Ratings.Add(rating);
            _db.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = rating.BookId });
        }

    }
}
