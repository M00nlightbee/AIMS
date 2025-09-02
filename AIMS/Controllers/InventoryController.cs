using AIMS.Data;
using Microsoft.AspNetCore.Mvc;
using AIMS.Models;
using Microsoft.AspNetCore.Http;
using System.Data;

namespace AIMS.Controllers
{
    public class InventoryController : Controller
    {
        //Access datamodel to handel CRUD operations on product table in DB
        private readonly ProductData _dataAccess;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public InventoryController(ProductData dataAccess, IHttpContextAccessor httpContextAccessor)
        {
            _dataAccess = dataAccess;
			_httpContextAccessor = httpContextAccessor;
		}

        // Index display Inventory (Get and Display)
        public IActionResult Index()
        {
			var role = _httpContextAccessor.HttpContext.Session.GetString("Role");
			if (role == "Admin" || role == "Manager")
			{
				var products = _dataAccess.GetProducts();
				return View(products);
			}
			else
			{
				return RedirectToAction("NoAccess", "Home");
			}
        }

        //Create product
        public IActionResult Create() 
        {
			var role = _httpContextAccessor.HttpContext.Session.GetString("Role");
			if (role == "Admin" || role == "Manager")
			{
				return View();
			}
			else
			{
				return RedirectToAction("NoAccess", "Home");
			}
		}

        [HttpPost]
        public IActionResult Create(Product product)
        {
			var role = _httpContextAccessor.HttpContext.Session.GetString("Role");
			if (role == "Admin" || role == "Manager")
			{
				if (ModelState.IsValid)
				{
					_dataAccess.CreateProduct(product);
					return RedirectToAction("Index");
				}
				return View(product);
			}
			else
			{
				return RedirectToAction("NoAccess", "Home");
			}

		}

		//Update Product
		//Get request to edit product
		public IActionResult Edit(int id)
        {
			var role = _httpContextAccessor.HttpContext.Session.GetString("Role");
			if (role == "Admin" || role == "Manager")
			{
				//Get product by id
				var products = _dataAccess.GetProductById(id);
				return View(products);
			}
			else
			{
				return RedirectToAction("NoAccess", "Home");
			}
	
		}

        [HttpPost]
        public IActionResult Edit(Product product, int id)
        {
			var role = _httpContextAccessor.HttpContext.Session.GetString("Role");
			if (role == "Admin" || role == "Manager")
			{
				if (ModelState.IsValid)
				{
					var existingProduct = _dataAccess.GetProductById(id);
					if (existingProduct == null)
					{
						return NotFound();
					}
					product.CreatedDate = existingProduct.CreatedDate;
					product.UpdatedDate = DateTime.Now; // Auto update updated date to be today

					_dataAccess.UpdateProduct(product, id);
					return RedirectToAction("Index");
				}
				return View(product);
			}
			else
			{
				return RedirectToAction("NoAccess", "Home");
			}
	
        }

        //Delete Product
        public IActionResult Delete()
        {
			var role = _httpContextAccessor.HttpContext.Session.GetString("Role");
			if (role == "Admin" || role == "Manager")
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
			if (role == "Admin" || role == "Manager")
			{
				_dataAccess.DeleteProduct(id);
				return RedirectToAction("Index");
			}
			else
			{
				return RedirectToAction("NoAccess", "Home");
			}

        }
    }
}
