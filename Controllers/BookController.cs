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
            List<Book> books;
            if (!string.IsNullOrEmpty(seriazedData))
            {
                books = System.Text.Json.JsonSerializer.Deserialize<List<Book>>(seriazedData, options);
            }
            else
            {
                books = _db.Books.ToList();
            }

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
            model.categories = _db.Categories.ToList();


            model.books = books;

            model.Pager.PagesCounter = (int)Math.Ceiling(model.books.Count() / (double)model.Pager.ItemsPerPage);
            int itemsPerPage = model.Pager.ItemsPerPage;
            int page = model.Pager.Page;
            model.books = books
                .Skip(itemsPerPage * (page - 1))
                .Take(itemsPerPage)
                .ToList();

            return View(model);
           
        }

      

        [FilterAttribute]
        public ActionResult Details(int id)
        {
            Book book = _db.Books.Include(b => b.Category).Include(b=>b.UsersWhoRead).SingleOrDefault(b => b.Id == id);
            BookDetailsVM bookDetailsVM = new BookDetailsVM();
            Rating rating = new Rating();

            bookDetailsVM.Readers = new List<User>();
            if(book.UsersWhoRead.Count > 0)
            {
                var sortedUser = book.UsersWhoRead.OrderByDescending(b => b.Id).Take(10);
                foreach(UserReadsBook urb in sortedUser)
                {
                    bookDetailsVM.Readers.Add(_db.Users.FirstOrDefault(u=>u.Id == urb.UserId));
                }
            }

            bookDetailsVM.Ratings = _db.Ratings.Where(r => r.BookId == id).Include(r=> r.User).ToList();
            bookDetailsVM.Book = book;
            bookDetailsVM.Rating = rating;
            if(bookDetailsVM != null)
            {
                return View(bookDetailsVM);
            }
            return RedirectToAction("Index");

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
        public ActionResult Create([Bind]Book model)
        {
            Console.WriteLine("Book Create action httppost is valid:" + ModelState.IsValid);
            List<Category> categories = _db.Categories.ToList();

            ViewBag.Categories = categories.Select(e => new SelectListItem(e.Title, e.Id.ToString()));
            Console.WriteLine("Author:" + model.Author);
            Console.WriteLine("ISBN:" + model.ISBN);;
            Console.WriteLine("Quantity:" + model.pages);
            Console.WriteLine("Title:" + model.Title);
            Console.WriteLine("YearOfPublish:" + model.YearOfPublish);
            Console.WriteLine("CategoryId:" + model.CategoryId);

            if (model.CategoryId > 0)
            {
                Category category = _db.Categories.Find(model.CategoryId);
      
                if (category != null)
                {
                    Console.WriteLine("Set category:" + category.Id);
                    model.Category = category;
                    model.CategoryId = category.Id;

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

            Book book = model;
            _db.Books.Add(book);
            _db.SaveChanges();
            Console.WriteLine("UserBook Create");
            return RedirectToAction("Index");
        }

        [FilterAttribute]
        [RoleFilter]
        [HttpGet]

        public ActionResult Edit(int id)
        {
            Book model = _db.Books.Find(id);

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
        public ActionResult Edit(int id, Book model)
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
                        List<Category> categories = _db.Categories.ToList();
                        ViewBag.Categories = categories.Select(e => new SelectListItem(e.Title, e.Id.ToString()));
                        return View(model);
                    }
                }

            }


            Book book = _db.Books.Find(id);
            if (book != null)
            {
                book.Title = model.Title;
                book.Author = model.Author;
                book.ISBN = model.ISBN;
                book.Category= model.Category;
                book.pages = model.pages;
                book.YearOfPublish = model.YearOfPublish;
                _db.SaveChanges();
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

            if(model.File == null)
            {
                return View(model);
            }

            string ext = Path.GetExtension(model.File.FileName);
            var fileName = $"model.FileName_{ DateTime.Now:ddMMyyyymms}{ext}" ;
           

            if(ext.ToLower() != ".pdf")
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
            if(book == null)
            {
                return RedirectToAction(nameof(Index)); 

            }

            bool alreadyRead = _db.UsersReadsBooks.Any(ur => ur.UserId == loggedUser.Id && ur.BookId == book.Id);
            
            if (!alreadyRead)
            {
                UserReadsBook userReadsBook = new UserReadsBook
                {
                    UserId = loggedUser.Id,
                    BookId = book.Id,
                };

                _db.UsersReadsBooks.Add(userReadsBook);
                _db.SaveChanges();
            }

            return View (book);
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

            return RedirectToAction(nameof(Details), new { id = rating.BookId});
        }

    }
}
