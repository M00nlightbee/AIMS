using System.Data.SqlClient;
using System.Diagnostics;
using AIMS.Data;
using AIMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductData _dataAccess;
        public HomeController(ProductData dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public IActionResult Index()
        {
            var productTotal = _dataAccess.TotalProduct;
            //var product = _dataAccess.GetProducts;
            return View(productTotal);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
