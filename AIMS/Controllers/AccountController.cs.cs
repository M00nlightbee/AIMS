using AIMS.Data;
using AIMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.UserPassword == password);

            if (user != null)
            {
                HttpContext.Session.SetString("Role", user.Role);

                if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Home"); // Redirect non-admin users to home
                }
            }

            ViewBag.Error = "Invalid credentials. Please try again.";
            return View();
        }


        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
