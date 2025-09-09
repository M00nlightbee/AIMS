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

		//Out of stock items
		public IActionResult OutOfStock()
		{
			return View();
		}

		//get item by id
		public IActionResult Create(int id)
        {
            var products = _dataAccessProduct.GetProductById(id);
            return View(products);
        }

		//post data to db
		//Add Item to Order if it does not exist, else update quantity
		[HttpPost]
		public IActionResult AddItem(Orders orders)
		{
			if (ModelState.IsValid)
			{
				// Check inventory quantity before adding item to order
				var product = _dataAccessProduct.GetProductById(orders.ProductId);
				if (product != null && product.Quantity > 0)
				{
					_dataAccess.AddItemToOrder(orders);
					return RedirectToAction("Index");
				}
				else
				{
					return RedirectToAction("OutOfStock");
				}
			}
			return View(orders);
		}

		//Update Quantity of Item in Order
		public IActionResult UpdateQuantity(int id)
        {
            var products = _dataAccess.GetOrderById(id);
            return View(products);
        }

		[HttpPost]
		public IActionResult UpdateQuantity(Orders order, int id)
		{
			if (ModelState.IsValid)
			{
				_dataAccess.UpdateOrder(order, id);
				return RedirectToAction("Index");
			}
			return View(order);
		}

		//Delete Item in Order
		//get view
		public IActionResult Delete()
        {
            return View();
        }

        //get by id and delete from db, post changes to db
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _dataAccess.RemoveItemFromOrder(id);
            return RedirectToAction("Index");
        }

		//Update Inventory Quantity
		public IActionResult UpdateInventory()
		{
			var orderDetails = _dataAccess.GetProductDetail();
			bool canUpdateInventory = true;

			// Check if there is enough stock for each item before updating inventory
			foreach (var order in orderDetails)
			{
				var product = _dataAccessProduct.GetProductById(order.ProductId);
				if (product != null && product.Quantity < order.OrderQuantity)
				{
					canUpdateInventory = false;
					break;
				}
			}
			// Update inventory if there is enough stock
			if (canUpdateInventory)
			{
				_dataAccess.UpdateInventoryQuantity();
				_dataAccess.ArchivedOrderDetails();
				return RedirectToAction("Index", "Inventory");
			}
			// Redirect to index if there is not enough stock
			else
			{
				return RedirectToAction("Index");
			}
		}

	}
}
