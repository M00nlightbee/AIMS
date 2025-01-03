using AIMS.Data;
using AIMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIMS.Controllers
{
    public class UserController : Controller
    {
		//Access datamodel to handel CRUD operations on user table in DB
		private readonly UserData _dataAccess;
        public UserController(UserData dataAccess)
        {
            _dataAccess = dataAccess;
        }

        // Index display users (Get and Display)
        public IActionResult Index()
        {
            var users = _dataAccess.GetUsers();
            return View(users);
        }

        //Create users
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Users user)
        {
            if (ModelState.IsValid)
            {
                _dataAccess.CreateUser(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        //Update User details
        public IActionResult Edit(int id)
        {
            var users = _dataAccess.GetUserById(id);
            return View(users);
        }

        [HttpPost]
        public IActionResult Edit(Users user, int id)
        {
            if (ModelState.IsValid)
            {
				//validate updated date before updating the product
				var existingUser = _dataAccess.GetUserById(id);
				if (existingUser != null && existingUser.CreatedDate == user.UpdatedDate)
				{
					ModelState.AddModelError("UpdatedDate", "Updated date cannot be the same as the created date.");
					return View(user);
				}
				_dataAccess.UpdateUser(user, id);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        //Delete User
        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _dataAccess.DeleteUser(id);
            return RedirectToAction("Index");
        }
    }
}
