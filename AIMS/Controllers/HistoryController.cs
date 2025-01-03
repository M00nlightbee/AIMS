using AIMS.Data;
using Microsoft.AspNetCore.Mvc;

namespace AIMS.Controllers
{
	public class HistoryController : Controller
	{
		//Data access for order history
		private readonly HistoryData _dataAccess;
		public HistoryController(HistoryData dataAccess)
		{
			_dataAccess = dataAccess;
		}

		//Display all order history
		public IActionResult Index()
		{
			var orderHistory = _dataAccess.GetOrderHistory();
			return View(orderHistory);
		}

	}
}
