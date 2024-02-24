using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Online_Library.Data;
using Online_Library.Filters;
using Online_Library.Models;

namespace Online_Library.Controllers
{
    public class UserController : Controller
    {
        private readonly LibraryDbContext _db;
        public UserController(LibraryDbContext db) //имаме методи които ще бъдат извикани 
        {
            _db = db;
        }
        // GET: UserController
        [FilterAttribute]
        [RoleFilterAttribute]
        public ActionResult Index()
        {
            List<User> objUserList = _db.Users.ToList();
            Console.WriteLine("User count: " + objUserList.Count);
            return View(objUserList);
        }
        [FilterAttribute]
        public ActionResult Details(int id)
        {
            User user = _db.Users.Find(id);
            if(user.Username != null)
            {
                return View(user);
            }
            return RedirectToAction("Index");
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            User user = model;
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;
            user.Password = "123";
            _db.Users.Add(user);
            _db.SaveChanges();
           return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            User model = _db.Users.Find(id);
            if (model.Username != null)
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(int id, User model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }


                User user = _db.Users.Find(id);
                if (user == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                user.Name = model.Name;
                user.Username = model.Username;
                user.Birthdate = model.Birthdate;
                user.UpdatedAt = DateTime.Now;
                user.Role = model.Role;
               // _db.Entry(user).State = EntityState.Modified;
                //_db.Users.Update(user);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Delete(int id)
        {
            User user = _db.Users.Find(id);
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }
            _db.Users.Remove(user);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

      
    }
}
