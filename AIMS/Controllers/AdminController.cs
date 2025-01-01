using Microsoft.AspNetCore.Mvc;
using AIMS.Data;
using AIMS.Models;

namespace AIMS.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserData _dataAccess;

        public AdminController(UserData dataAccess)
        {
            _dataAccess = dataAccess;
        }

        // GET: List all users
        [HttpGet]
        public IActionResult Index()
        {
            var users = _dataAccess.GetUsers(); // Fetch all users from the database
            return View(users);
        }

        // GET: Create User Page
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        // POST: Create User
        [HttpPost]
        public IActionResult CreateUser(Users user)
        {
            if (ModelState.IsValid)
            {
                user.CreatedDate = DateTime.Now;
                user.UpdatedDate = DateTime.Now;

                _dataAccess.CreateUser(user); // Save user to the database

                TempData["SuccessMessage"] = "User created successfully!";
                return RedirectToAction("Index"); // Redirect to the users list
            }

            return View(user);
        }
    }
}
