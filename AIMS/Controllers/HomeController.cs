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
			var analytics = new Analytics
			{
				TotalQuantity = _dataAccess.GetTotalQuantity().TotalQuantity,
				StockByBranch = _dataAccess.GetStockByBranch(),
				StockByCategory = _dataAccess.GetStockByCategory()
			};

			return View(analytics);
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
