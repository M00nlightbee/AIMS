using System.Data;
using System.Data.Common;
using System.Globalization;
using AIMS.Data;
using AIMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIMS.Controllers
{
    public class OrderController : Controller
    {
        //Access datamodel to handel CRUD operations in DB
        private readonly OrderData _dataAccess;
        private readonly ProductData _dataAccessProduct;
        public OrderController(OrderData dataAccess, ProductData dataAccessProduct)
        {
            _dataAccess = dataAccess;
            _dataAccessProduct = dataAccessProduct;
        }

        //display all added item in table
        public IActionResult Index()
        {
            var orderDetails = _dataAccess.GetProductDetail();
            return View(orderDetails);
        }

        //get item by id
        public IActionResult Create(int id)
        {
            var products = _dataAccessProduct.GetProductById(id);
            return View(products);
        }

        //post data to db
        [HttpPost]
        public IActionResult AddItem(Orders orders)
        {
            if (ModelState.IsValid)
            {
                _dataAccess.AddItemToOrder(orders);
                ViewBag.Message = "Item Successfully Added";
                return RedirectToAction("Index");
            }           
            return View();
        }

        //Delete Item in Order
        //get view
        public IActionResult Delete()
        {
            return View();
        }

        // get by id and delete from db, post changes to db
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _dataAccess.RemoveItemFromOrder(id);
            ViewBag.Message = "Item Successfully Removed";
            return RedirectToAction("Index");
        }
    }
}
