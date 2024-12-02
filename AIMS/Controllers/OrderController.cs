using Microsoft.AspNetCore.Mvc;

namespace AIMS.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
