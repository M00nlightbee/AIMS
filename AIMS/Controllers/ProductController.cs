using AIMS.Data;
using AIMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIMS.Controllers
{
    public class ProductController : Controller
    {
		//Access datamodel to handel CRUD operations on product table in DB
		private readonly ProductData _dataAccess;
        public ProductController(ProductData dataAccess)
        {
            _dataAccess = dataAccess;
        }

        //Display all product cards
        public IActionResult Index()
        {
            var products = _dataAccess.GetProducts();
            return View(products);
        }

    }
}
