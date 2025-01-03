using AIMS.Data;
using Microsoft.AspNetCore.Mvc;
using AIMS.Models;

namespace AIMS.Controllers
{
    public class InventoryController : Controller
    {
        //Access datamodel to handel CRUD operations on product table in DB
        private readonly ProductData _dataAccess;
        public InventoryController(ProductData dataAccess)
        {
            _dataAccess = dataAccess;
        }

        // Index display Inventory (Get and Display)
        public IActionResult Index()
        {
            var products = _dataAccess.GetProducts();
            return View(products);
        }

        //Create product
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
				_dataAccess.CreateProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
		}

		//Update Product
		//Get request to edit product
		public IActionResult Edit(int id)
        {
			//Get product by id
			var products = _dataAccess.GetProductById(id);
            return View(products);
		}

        [HttpPost]
        public IActionResult Edit(Product product, int id)
        {
            if (ModelState.IsValid)
            {
				//validate updated date before updating the product
				var existingProduct = _dataAccess.GetProductById(id);
				if (existingProduct != null && existingProduct.CreatedDate == product.UpdatedDate)
				{
					ModelState.AddModelError("UpdatedDate", "Updated date cannot be the same as the created date.");
                    return View(product);
				}
				_dataAccess.UpdateProduct(product, id);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        //Delete Product
        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _dataAccess.DeleteProduct(id);
            return RedirectToAction("Index");
        }
    }
}
