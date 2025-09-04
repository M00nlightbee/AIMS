using AIMS.Models;
using AIMS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AIMS.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserService _userService;

        public LoginController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string email, string password)
        {
            var user = _userService.ValidateUser(email, password);

            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.FirstName + " " + user.LastName);
                HttpContext.Session.SetString("Role", user.Role);
                return RedirectToAction("Index", "Product");
            }

            ViewBag.Message = "Invalid email or password.";
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
			TempData["LoggedOut"] = true;
			HttpContext.Session.Clear();
			return RedirectToAction("Index");

        }
    }
}
