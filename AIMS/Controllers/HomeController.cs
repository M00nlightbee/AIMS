using System.Data;
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
		private readonly IHttpContextAccessor _httpContextAccessor;
		public HomeController(ProductData dataAccess, IHttpContextAccessor httpContextAccessor)
		{
			_dataAccess = dataAccess;
			_httpContextAccessor = httpContextAccessor;
		}

		public IActionResult Index()
		{
			var role = _httpContextAccessor.HttpContext.Session.GetString("Role");
			if (role == "Admin")
			{
				var analytics = new Analytics
				{
					TotalQuantity = _dataAccess.GetTotalQuantity().TotalQuantity,
					StockByBranch = _dataAccess.GetStockByBranch(),
					StockByCategory = _dataAccess.GetStockByCategory()
				};

				return View(analytics);
			}
			else
			{
				return RedirectToAction("NoAccess", "Home");
			}
	
		}

		public IActionResult NoAccess()
		{
			return View();
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
