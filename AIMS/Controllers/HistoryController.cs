using System.Data;
using AIMS.Data;
using Microsoft.AspNetCore.Mvc;

namespace AIMS.Controllers
{
	public class HistoryController : Controller
	{
		//Data access for order history
		private readonly HistoryData _dataAccess;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public HistoryController(HistoryData dataAccess, IHttpContextAccessor httpContextAccessor)
		{
			_dataAccess = dataAccess;
			_httpContextAccessor = httpContextAccessor;
		}

		//Display all order history
		public IActionResult Index()
		{
			var role = _httpContextAccessor.HttpContext.Session.GetString("Role");
			if (role == "Admin")
			{
				var orderHistory = _dataAccess.GetOrderHistory();
				return View(orderHistory);
			}
			else
			{
				return RedirectToAction("NoAccess", "Home");
			}

		}

	}
}
