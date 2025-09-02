using AIMS.Data;
using AIMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIMS.Controllers
{
    public class UserController : Controller
    {
		//Access datamodel to handel CRUD operations on user table in DB
		private readonly UserData _dataAccess;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public UserController(UserData dataAccess, IHttpContextAccessor httpContextAccessor)
        {
            _dataAccess = dataAccess;
			_httpContextAccessor = httpContextAccessor;

		}

		// Index display users (Get and Display)
		public IActionResult Index()
		{
			var role = _httpContextAccessor.HttpContext.Session.GetString("Role");
			if (role  == "Admin")
			{
				var users = _dataAccess.GetUsers();
				return View(users);
			}
			else
			{
				return RedirectToAction("NoAccess", "Home");
			}
		}

		//Create users
		public IActionResult Create()
		{
			var role = _httpContextAccessor.HttpContext.Session.GetString("Role");
			if (role == "Admin")
			{
				return View();
			}
			else
			{
				return RedirectToAction("NoAccess", "Home");
			}	
		}

		[HttpPost]

		public IActionResult Create(Users user)
		{
			var role = _httpContextAccessor.HttpContext.Session.GetString("Role");
			if (role == "Admin")
			{
				if (ModelState.IsValid)
				{
					_dataAccess.CreateUser(user);
					TempData["UserAdded"] = true;
					return RedirectToAction("Index");
					
				}
				return View(user);
			}
			else
			{
				return RedirectToAction("NoAccess", "Home");
			}
			
		}

		//Update User details
		public IActionResult Edit(int id)
		{
			var role = _httpContextAccessor.HttpContext.Session.GetString("Role");
			if (role == "Admin")
			{
				var users = _dataAccess.GetUserById(id);
				return View(users);
			}
			else
			{
				return RedirectToAction("NoAccess", "Home");
			}

		}

		[HttpPost]
		public IActionResult Edit(Users user, int id)
		{
			var role = _httpContextAccessor.HttpContext.Session.GetString("Role");
			if (role == "Admin")
			{
				if (ModelState.IsValid)
				{

					//validate updated date before updating the user
					var existingUser = _dataAccess.GetUserById(id);
				
					if (existingUser == null)
					{
						return NotFound();
					}
					user.CreatedDate = existingUser.CreatedDate;
					user.UpdatedDate = DateTime.Now; // Auto update updated date to be today
					_dataAccess.UpdateUser(user, id);
					return RedirectToAction("Index");
				}
				return View(user);
			}
			else
			{
				return RedirectToAction("NoAccess", "Home");
			}

		}
		//Delete User
		public IActionResult Delete()
		{
			var role = _httpContextAccessor.HttpContext.Session.GetString("Role");
			if (role == "Admin")
			{
				return View();
			}
			else
			{
				return RedirectToAction("NoAccess", "Home");
			}
		}

		[HttpPost]
		public IActionResult Delete(int id)
		{
			var role = _httpContextAccessor.HttpContext.Session.GetString("Role");
			if (role == "Admin")
			{
				_dataAccess.DeleteUser(id);
				return RedirectToAction("Index");
			}
			else
			{
				return RedirectToAction("NoAccess", "Home");
			}

		}
	}
}
