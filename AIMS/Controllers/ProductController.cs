using AIMS.Data;
using Microsoft.AspNetCore.Mvc;

namespace AIMS.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductData _dataAccess;
        public ProductController(ProductData dataAccess)
        {
            _dataAccess = dataAccess;
        }

        //Display page index
        public IActionResult Index()
        {
            var products = _dataAccess.GetProducts();
            return View(products);
        }

        //Get specific prodcut using ID: product/details/1
        public IActionResult Details(int id)
        {
            var products = _dataAccess.GetProductById(id);
            return View(products);
        }
    }
}
