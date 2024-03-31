using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Library.Data;
using Online_Library.Models;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Online_Library.Controllers
{
    public class SharedController : Controller
    {
        private readonly LibraryDbContext _db;
        public SharedController(LibraryDbContext db)
        {
            _db = db;
        }   

        public IActionResult Search(string query)
        {
            if (!string.IsNullOrEmpty(query))
            {
                string lowerQu = query.ToLower();
                List<Book> books = _db.Books
                    .Include(b => b.Category)
                    .Where(b =>
                    b.Title.ToLower().Contains(lowerQu) ||
                    b.Author.ToLower().Contains(lowerQu) ||
                    b.Description.ToLower().Contains(lowerQu)
                    ).ToList();
                //добавяме настройки на серилизацията за да избегнем циклични повторения
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    MaxDepth = 64
                };
                // Сериализация на обекта книги към json формат
                var serializedData = JsonSerializer.Serialize(books,options);
                TempData["Books"] = serializedData;
                return RedirectToAction("Catalog", "Book");
            }

            return View();
        }

        public IActionResult FilterCategory(int category)
        {
            if (category > 0 )
            {
                List<Book> books = _db.Books
                    .Include(b => b.Category)
                    .Where(b => b.Category.Id == category
                    ).ToList();
                //добавяме настройки на серилизацията за да избегнем циклични повторения
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    MaxDepth = 64
                };
                // Сериализация на обекта книги към json формат
                var serializedData = JsonSerializer.Serialize(books, options);
                TempData["Books"] = serializedData;
                return RedirectToAction("Catalog", "Book");
            }

            return View();
        }
    }
}
