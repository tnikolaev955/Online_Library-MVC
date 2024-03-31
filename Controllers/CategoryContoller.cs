using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Online_Library.Data;
using Online_Library.Filters;
using Online_Library.Models;
using Online_Library.View_Model;



namespace Online_Library.Controllers
{
 
    public class CategoryController : Controller
    {
        private readonly LibraryDbContext _db;


        public CategoryController(LibraryDbContext db)
        {
            _db = db;
        }

        [RoleFilterAttribute]
        public ActionResult Index()
        {
            List<Category> categories = _db.Categories.ToList();
            
            return View(categories);
        }


        [HttpGet]
        public ActionResult Create()
        {
            List<Category> categories = _db.Categories.ToList();
            CategoryCreateVM createVM = new CategoryCreateVM();
            createVM.Categories = categories;
            return View(createVM);
        }

        [HttpPost]
        public ActionResult Create(CategoryCreateVM model)
        {
            if (!ModelState.IsValid)
            {

                List<Category> categories = _db.Categories.ToList();
                model.Categories = categories;
                return View(model);
            }
            Category category = new Category
            { 
                Description = model.Category.Description,
                Title = model.Category.Title,
                ParentId = model.Category.ParentId,
                ParentCategory = _db.Categories.FirstOrDefault(c => c.Id == model.Category.ParentId ),
        
            };

            _db.Categories.Add(category);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {

            List<Category> categories = _db.Categories.ToList();
            CategoryCreateVM createVM = new CategoryCreateVM();
            createVM.Categories = categories;

            Category category = _db.Categories.FirstOrDefault(c => c.Id == id);
            if(category == null)
            {
                return RedirectToAction("Index");
            }
            createVM.Category = category;
            return View(createVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CategoryCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                List<Category> categories = _db.Categories.ToList();
                model.Categories = categories;
                return View(model);
            }
            Category category = _db.Categories.FirstOrDefault(c => c.Id == id);

            if(category != null)
            {
                category.Id = model.Category.Id;
                category.Description = model.Category.Description;
                category.Title = model.Category.Title;
                category.ParentId = model.Category.ParentId;
                category.ParentCategory = _db.Categories.FirstOrDefault(c => c.Id == model.Category.ParentId);
            }


            _db.Categories.Update(category);
            _db.SaveChanges();

            return RedirectToAction("Index");   
        }

        public ActionResult Delete(int id)
        {
            Category category = _db.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _db.Categories.Remove(category);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }


    }
}
