using AIMS.Controllers;
using AIMS.Data;
using AIMS.Models;
using AIMS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Session;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace TestProject1
{
    public class MockHttpSession : ISession
    {
        readonly Dictionary<string, object> _sessionStorage = new Dictionary<string, object>();
        string ISession.Id => throw new NotImplementedException();
        bool ISession.IsAvailable => throw new NotImplementedException();
        IEnumerable<string> ISession.Keys => _sessionStorage.Keys;
        void ISession.Clear()
        {
            _sessionStorage.Clear();
        }
        Task ISession.CommitAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        Task ISession.LoadAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        void ISession.Remove(string key)
        {
            _sessionStorage.Remove(key);
        }
        void ISession.Set(string key, byte[] value)
        {
            _sessionStorage[key] = Encoding.UTF8.GetString(value);
        }
        bool ISession.TryGetValue(string key, out byte[] value)
        {
            if (_sessionStorage[key] != null)
            {
                value = Encoding.ASCII.GetBytes(_sessionStorage[key].ToString());
                return true;
            }
            value = null;
            return false;
        }
    };

    
    
    public class Tests
    {
        //static string jsonFile = "C:\\Users\\Corsa\\Downloads\\AIMS-master\\AIMS-master\\AIMS\\appsettings.json";
        static public IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();

        //var role = _httpContextAccessor.HttpContext.Session.GetString("Role");
        static public HttpContextAccessor newContext = new HttpContextAccessor();


        //[SetUp]
        public void Setup()
        {
        }

        public class HomeController_Test
        {

            //Index
            [Test]
            public void IActionIndexTest_HomeController()
            {
                //HttpContext http = new DefaultHttpContext(new IConfiguration());
                //ConfigurationManager configurationManager = new ConfigurationManager();

                //HttpContext.Session.SetString("Role", "Admin");

                AIMS.Data.ProductData proData = new ProductData(config);
                ViewResult? result = new HomeController(proData, (IHttpContextAccessor)MockHttpContext()).Index() as ViewResult;
                Assert.IsNotNull(result.ViewData);
                Assert.IsNotEmpty((result.ViewData.Model as Analytics).ToString());
            }

            //Privacy
            [Test]
            public void IActionPrivacyTest_HomeController()
            {
                AIMS.Data.ProductData proData = new ProductData(config);
                var result = new HomeController(proData, (IHttpContextAccessor)MockHttpContext()).Privacy() as ViewResult;
                Assert.IsNotNull(result.ViewData);
            //var role = _httpContextAccessor.HttpContext.Session.GetString("Role");
                
            }

            //Error
            [Test]
            public void IActionErrorTest_HomeController()
            {
                AIMS.Data.ProductData proData = new ProductData(config);
                HomeController homeController = new HomeController(proData, (IHttpContextAccessor)MockHttpContext());
                homeController.ControllerContext = new ControllerContext();
                homeController.ControllerContext.HttpContext = new DefaultHttpContext();

                var result = homeController.Error() as ViewResult;
                Assert.IsNull(result.ViewName);
                Assert.True(result.ViewData.Model.GetType().ToString().Contains("ErrorViewModel"));
            }
        }

        public class InventoryController_Test
        {
            //Index
            [Test]
            public void IActionIndexTest_InventoryController()
            {
                AIMS.Data.ProductData proData = new ProductData(config);
                AIMS.Controllers.InventoryController newInventController = new InventoryController(proData, (IHttpContextAccessor)MockHttpContext());
                ViewResult result = newInventController.Index() as ViewResult;
                List<Product> resultData = result.ViewData.Model as List<Product>;


                Assert.IsNotNull(resultData);
                Assert.That(resultData.Count >= 0);
            }

            //Create
            [Test]
            public void IActionCreateTest_InventoryController_Empty()
            {
                AIMS.Data.ProductData proData = new ProductData(config);
                AIMS.Controllers.InventoryController newInventController = new InventoryController(proData, (IHttpContextAccessor)MockHttpContext());
                var result = newInventController.Create() as ViewResult;
                Assert.IsNull(result.ViewName);
                Assert.IsEmpty(result.ViewData);
            }

            [Test]
            public void IActionCreateTest_InventoryController_Parameters_Product()
            {
                AIMS.Data.ProductData proData = new ProductData(config);
                AIMS.Controllers.InventoryController newInventController = new InventoryController(proData, (IHttpContextAccessor)MockHttpContext());
                AIMS.Models.Product product = new AIMS.Models.Product { 
                    Branch = "London", 
                    Category = "Accessories", 
                    Price = 20, 
                    ProductDescription = "cool looking clothing",
                    ProductName = "Nice hat",
                    ProductSize = "ONE SIZE",
                    Quantity = 30,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };

                Object final = false;
                newInventController.ModelState.SetModelValue("IsValid", final, "false");

                var result = (ViewResult)newInventController.Create(product);
                var productResult = result.ViewData.Model as Product;
         
                Assert.AreEqual(product.ProductName, productResult.ProductName);

            }

            [Test]
            public void IActionCreateTest_InventoryController_Parameters_Index()
            {
                AIMS.Data.ProductData proData = new ProductData(config);
                AIMS.Controllers.InventoryController newInventController = new InventoryController(proData, (IHttpContextAccessor)MockHttpContext());
               
                AIMS.Models.Product product = new AIMS.Models.Product
                {
                    Branch = "London",
                    Category = "Accessories",
                    Price = 20,
                    ProductDescription = "cool looking clothing",
                    ProductName = "Nice hat",
                    ProductSize = "ONE SIZE",
                    Quantity = 30,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };
                var result = newInventController.Create(product) as RedirectToActionResult;
                Assert.AreEqual("Index", result.ActionName);
            }

            //Edit
            [Test]
            public void IActionEditTest_InventoryController_Int()
            {
                AIMS.Data.ProductData proData = new ProductData(config);
                AIMS.Controllers.InventoryController newInventController = new InventoryController(proData, (IHttpContextAccessor)MockHttpContext());
                AIMS.Models.Product product = new AIMS.Models.Product
                {
                    Branch = "London",
                    Category = "Accessories",
                    Price = 20,
                    ProductDescription = "cool looking clothing",
                    ProductName = "Nice hat",
                    ProductSize = "ONE SIZE",
                    Quantity = 30,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };

                newInventController.Create(product);
                List<Product> newProduct = (newInventController.Index() as ViewResult).ViewData.Model as List<Product>;
                int newProductID = newProduct.Last().ProductId;
                Product? result = (newInventController.Edit(newProductID) as ViewResult).ViewData.Model as Product;
                Assert.IsNotNull(result);
            }

            [Test]
            public void IActionEditTest_InventoryController_Parameters_Index()
            {
                AIMS.Data.ProductData proData = new ProductData(config);
                AIMS.Controllers.InventoryController newInventController = new InventoryController(proData, (IHttpContextAccessor)MockHttpContext());
                AIMS.Models.Product product = new AIMS.Models.Product
                {
                    Branch = "London",
                    Category = "Accessories",
                    Price = 20,
                    ProductDescription = "cool looking clothing",
                    ProductName = "Nice hat",
                    ProductSize = "ONE SIZE",
                    Quantity = 30,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };

                newInventController.Create(product);
                int newProductID = ((newInventController.Index() as ViewResult).ViewData.Model as List<Product>).Last().ProductId;
                
                var result = newInventController.Edit(product, newProductID) as RedirectToActionResult;
                Assert.AreEqual("Index", result.ActionName);
            }

            [Test] 
            public void IActionEditTest_InventoryController_Parameters_Product()
            {

                AIMS.Data.ProductData proData = new ProductData(config);
                AIMS.Controllers.InventoryController newInventController = new InventoryController(proData, (IHttpContextAccessor)MockHttpContext());
                AIMS.Models.Product product = new AIMS.Models.Product
                {
                    Branch = "London",
                    Category = "Accessories",
                    Price = 20,
                    ProductDescription = "cool looking clothing",
                    ProductName = "Nice hat",
                    ProductSize = "ONE SIZE",
                    Quantity = 30,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };

                newInventController.Create(product);
                int newProductID = ((newInventController.Index() as ViewResult).ViewData.Model as List<Product>).Last().ProductId;

                Object final = false;
                newInventController.ModelState.SetModelValue("IsValid", final, "false");

                var result = newInventController.Edit(product,newProductID) as ViewResult;
                var resultData = result.ViewData.Model as Product;

                Assert.AreEqual("ViewResult", result.GetType().Name.ToString());
                Assert.AreEqual(product.ProductName, resultData.ProductName);
                
                
            }

            [Test]
            public void IActionEditTest_InventoryController_Parameters_ModelError()
            {
                DateTime dateTime = DateTime.Today;
                AIMS.Data.ProductData proData = new ProductData(config);
                AIMS.Controllers.InventoryController newInventController = new InventoryController(proData, (IHttpContextAccessor)MockHttpContext());
                AIMS.Models.Product product = new AIMS.Models.Product
                {
                    Branch = "London",
                    Category = "Accessories",
                    Price = 20,
                    ProductDescription = "cool looking clothing",
                    ProductName = "Nice hat",
                    ProductSize = "ONE SIZE",
                    Quantity = 30,
                    CreatedDate = dateTime,
                    UpdatedDate = dateTime,
                };

                //created new product
                newInventController.Create(product);
                List<Product> newVariable = (newInventController.Index() as ViewResult).ViewData.Model as List<Product>;
                Product proToCheck = newVariable.Last();

                var result = newInventController.Edit(product, proToCheck.ProductId) as ViewResult;
                var resultModelState = result.ViewData.ModelState;
                Assert.That(resultModelState.IsValid, Is.False);
                Assert.That(resultModelState.Count > 0, Is.True);

                var resultData = result.ViewData.Model as Product;

                Assert.AreEqual("ViewResult", result.GetType().Name.ToString());
                Assert.AreEqual(product.ProductName, resultData.ProductName);
            }

            //Delete
            [Test]
            public void IActionDeleteTest_InventoryController_Empty()
            {
                AIMS.Data.ProductData proData = new ProductData(config);
                AIMS.Controllers.InventoryController newInventController = new InventoryController(proData, (IHttpContextAccessor)MockHttpContext());
                AIMS.Models.Product product = new AIMS.Models.Product
                {
                    Branch = "London",
                    Category = "Accessories",
                    Price = 20,
                    ProductDescription = "cool looking clothing",
                    ProductName = "Nice hat",
                    ProductSize = "ONE SIZE",
                    Quantity = 30,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };
                var result = newInventController.Delete() as ViewResult;
                Assert.IsNull(result.Model);
            }

            [Test] 
            public void IActionDeleteTest_InventoryController_Parameters()
            {
                AIMS.Data.ProductData proData = new ProductData(config);
                AIMS.Controllers.InventoryController newInventController = new InventoryController(proData, (IHttpContextAccessor)MockHttpContext());
                AIMS.Models.Product product = new AIMS.Models.Product
                {
                    Branch = "London",
                    Category = "Accessories",
                    Price = 20,
                    ProductDescription = "cool looking clothing",
                    ProductName = "Nice hat",
                    ProductSize = "ONE SIZE",
                    Quantity = 30,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };

                newInventController.Create(product);
                int newProductID = ((newInventController.Index() as ViewResult).ViewData.Model as List<Product>).Last().ProductId;
                var result = newInventController.Delete(newProductID) as RedirectToActionResult;
                Assert.AreEqual("Index", result.ActionName);
            }
        }

        public class OrderController_Test
        {
            //Index
            [Test]
            public void IActionIndexTest_OrderController_Empty()
            {
                AIMS.Data.ProductData proData = new ProductData(config);
                OrderData orData = new OrderData(config);
                OrderController orControl = new OrderController(orData, proData);

                
                var result = orControl.Index() as ViewResult;
                var resultData = result.ViewData.Model;
                Assert.That(resultData, Is.Not.Null);
                
            }

            //Out of stock
            [Test]
            public void IActionOOSTest_OrderController()
            {
                AIMS.Data.ProductData proData = new ProductData(config);
                OrderData orData = new OrderData(config);
                OrderController orControl = new OrderController(orData, proData);
                ViewResult? result = orControl.OutOfStock() as ViewResult;
                Assert.IsNull(result.Model);
                Assert.That(result.GetType().ToString().Contains( "ViewResult"));
            }

            //Create
            [Test]
            public void IActionCreateTest_OrderController_Int()
            {
                AIMS.Data.ProductData proData = new ProductData(config);
                OrderData orData = new OrderData(config);
                OrderController orControl = new OrderController(orData, proData);
                AIMS.Models.Product product = new AIMS.Models.Product
                {
                    Branch = "London",
                    Category = "Accessories",
                    Price = 20,
                    ProductDescription = "cool looking clothing",
                    ProductName = "Nice hat",
                    ProductSize = "ONE SIZE",
                    Quantity = 10,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };

                InventoryController newInventController = new InventoryController(proData, (IHttpContextAccessor)MockHttpContext());
                newInventController.Create(product);

                List<Product> newProduct = (newInventController.Index() as ViewResult).ViewData.Model as List<Product>;
                int newProductID = newProduct.Last().ProductId;

                var result = orControl.Create(newProductID) as ViewResult;
                Assert.IsTrue(result.ViewData.Model.GetType().ToString().ToLower().EndsWith("product"));
            }

            //AddItem
            [Test]
            public void IActionAddItemTest_OrderController_Parameters_Index()
            {
                AIMS.Data.ProductData proData = new ProductData(config);
                OrderData orData = new OrderData(config);
                OrderController orControl = new OrderController(orData, proData);

                AIMS.Models.Product product = new AIMS.Models.Product
                {
                    Branch = "London",
                    Category = "Accessories",
                    Price = 20,
                    ProductDescription = "cool looking clothing",
                    ProductName = "Nice hat",
                    ProductSize = "ONE SIZE",
                    Quantity = 10,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };
                InventoryController productController = new InventoryController(proData, (IHttpContextAccessor)MockHttpContext());
                productController.Create(product);
                List<Product> newList = (productController.Index() as ViewResult).ViewData.Model as List<Product>;
                Product centralProduct = newList.Last();



                productController.Edit(product, centralProduct.ProductId);

                AIMS.Models.Orders orders = new AIMS.Models.Orders
                {
                    CreatedAt = DateTime.Now,
                    OrderQuantity = 2,
                    ProductId = centralProduct.ProductId,
                };

                var result = orControl.AddItem(orders) as RedirectToActionResult;
                Assert.AreEqual("Index", result.ActionName);

            }

            [Test]
            public void IActionAddItemTest_OrderController_Parameters_OOS()
            {
                AIMS.Data.ProductData proData = new ProductData(config);
                OrderData orData = new OrderData(config);
                AIMS.Models.Product product = new AIMS.Models.Product
                {
                    Branch = "London",
                    Category = "Accessories",
                    Price = 20,
                    ProductDescription = "cool looking clothing",
                    ProductName = "Nice hat",
                    ProductSize = "ONE SIZE",
                    Quantity = 0,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };
                InventoryController newInventController = new InventoryController(proData, (IHttpContextAccessor)MockHttpContext());
                newInventController.Create(product);
                int newProductID = ((newInventController.Index() as ViewResult).ViewData.Model as List<Product>).Last().ProductId;


                OrderController orControl = new OrderController(orData, proData);
                AIMS.Models.Orders orders = new AIMS.Models.Orders
                {
                    CreatedAt = DateTime.Now,
                    OrderQuantity = 2,
                    ProductId = newProductID,
                };
                
                var result = orControl.AddItem(orders) as RedirectToActionResult;
                Assert.AreEqual("OutOfStock", result.ActionName);

            }

            [Test]
            public void IActionAddItemTest_OrderController_Parameters_View()
            {
                AIMS.Data.ProductData proData = new ProductData(config);
                OrderData orData = new OrderData(config);
                OrderController orControl = new OrderController(orData, proData);
                InventoryController newInventController = new InventoryController(proData, (IHttpContextAccessor)MockHttpContext());
                int newProductID = ((newInventController.Index() as ViewResult).ViewData.Model as List<Product>).Last().ProductId;
                AIMS.Models.Orders orders = new AIMS.Models.Orders
                {
                    CreatedAt = DateTime.Now,
                    OrderQuantity = 2,
                    ProductId = newProductID,
                };

                Object final = false;
                orControl.ModelState.SetModelValue("IsValid", final, "false");

                var result = orControl.AddItem(orders) as ViewResult;
                var resultData = result.ViewData.Model;
                Assert.AreEqual(resultData, orders);

            }

            //UpdateQuantity
            [Test]
            public void IActionUpdateQuantityTest_OrderController_Int()
            {
                AIMS.Data.ProductData proData = new ProductData(config);
                OrderData orData = new OrderData(config);
                OrderController orControl = new OrderController(orData, proData);

                AIMS.Models.Product product = new AIMS.Models.Product
                {
                    Branch = "London",
                    Category = "Accessories",
                    Price = 20,
                    ProductDescription = "cool looking clothing",
                    ProductName = "Nice hat",
                    ProductSize = "ONE SIZE",
                    Quantity = 10,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };
                InventoryController inventController = new InventoryController(proData, (IHttpContextAccessor)MockHttpContext());
                inventController.Create(product);
                List<Product> newList = (inventController.Index() as ViewResult).ViewData.Model as List<Product>;
                Product centralProduct = newList.Last();
                
                
                AIMS.Models.Orders orders = new AIMS.Models.Orders
                {
                    CreatedAt = DateTime.Now,
                    OrderQuantity = 1,
                    ProductId = centralProduct.ProductId,
                };


                orControl.AddItem(orders);
                
                int newOrderID = ((orControl.Index() as ViewResult).ViewData.Model as List<OrderDetails>).Last().OrderId;

                Orders result = (orControl.UpdateQuantity(newOrderID) as ViewResult).ViewData.Model as Orders;
                Orders result2 = (orControl.UpdateQuantity(newOrderID) as ViewResult).ViewData.Model as Orders;

                if (result is not null)
                {
                    Assert.That(result2.OrderQuantity!, Is.LessThanOrEqualTo(result.OrderQuantity));
                }
                else
                {
                    throw new Exception("There are no orders stored in the database");
                }
            }

            [Test]
            public void IActionUpdateQuantityTest_OrderController_Parameters_Index()
            {
                AIMS.Data.ProductData proData = new ProductData(config);
                OrderData orData = new OrderData(config);
                OrderController orControl = new OrderController(orData, proData);
                InventoryController newInventController = new InventoryController(proData, (IHttpContextAccessor)MockHttpContext());
                int newProductID = ((newInventController.Index() as ViewResult).ViewData.Model as List<Product>).Last().ProductId;

                AIMS.Models.Orders orders = new AIMS.Models.Orders
                {
                    CreatedAt = DateTime.Now,
                    OrderQuantity = 2,
                    ProductId = newProductID,
                };

                var result = orControl.UpdateQuantity(orders, newProductID) as RedirectToActionResult;
                Assert.AreEqual("Index", result.ActionName);

            }

            [Test]
            public void IActionUpdateQuantityTest_OrderController_Parameters_Order()
            {
                DateTime current = DateTime.Now;
                AIMS.Data.ProductData proData = new ProductData(config);
                OrderData orData = new OrderData(config);
                OrderController orControl = new OrderController(orData, proData);
                InventoryController newInventController = new InventoryController(proData, (IHttpContextAccessor)MockHttpContext());
                int newProductID = ((newInventController.Index() as ViewResult).ViewData.Model as List<Product>).Last().ProductId;

                AIMS.Models.Orders orders = new AIMS.Models.Orders
                {
                    CreatedAt = current,
                    OrderQuantity = 2,
                    ProductId = newProductID,
                };
                
                Object final = false;
                orControl.ModelState.SetModelValue("IsValid", final, "false");
                
                var result = orControl.UpdateQuantity(orders, newProductID) as ViewResult;
                var resultData = result.ViewData.Model as Orders;
                Assert.AreEqual(resultData.CreatedAt, orders.CreatedAt);
                Assert.AreEqual("ViewResult", result.GetType().Name.ToString());
                Assert.NotNull(result);

            }

            //DeleteItem
            [Test]
            public void IActionDelete_OrderController_Empty()
            {
                AIMS.Data.ProductData proData = new ProductData(config);
                OrderData orData = new OrderData(config);
                OrderController orControl = new OrderController(orData, proData);
                InventoryController newInventController = new InventoryController(proData, (IHttpContextAccessor)MockHttpContext());
                int newProductID = ((newInventController.Index() as ViewResult).ViewData.Model as List<Product>).Last().ProductId;
                AIMS.Models.Orders orders = new AIMS.Models.Orders
                {
                    CreatedAt = DateTime.Now,
                    OrderQuantity = 2,
                    ProductId = newProductID,
                };
                var result = orControl.Delete() as ViewResult;

                //returned unnamed view
                Assert.That(result.ViewName, Is.Null);
                Assert.That(result, Is.Not.Null);
                
            }

            [Test]
            public void IActionDelete_OrderController_Int()
            {
                AIMS.Data.ProductData proData = new ProductData(config);
                OrderData orData = new OrderData(config);
                OrderController orControl = new OrderController(orData, proData);
                InventoryController newInventController = new InventoryController(proData, (IHttpContextAccessor)MockHttpContext());
                int newProductID = ((newInventController.Index() as ViewResult).ViewData.Model as List<Product>).Last().ProductId;
                AIMS.Models.Orders orders = new AIMS.Models.Orders
                {
                    CreatedAt = DateTime.Now,
                    OrderQuantity = 2,
                    ProductId = newProductID,
                };

                orControl.AddItem(orders);
                int lastOrderID = ((orControl.Index() as ViewResult).ViewData.Model as List<OrderDetails>).Last().OrderId;

                var result = orControl.Delete(lastOrderID) as RedirectToActionResult;
                Assert.AreEqual("Index", result.ActionName);
            }

            //UpdateInventory
            [Test]
            public void IActionUpdateInventory_OrderController_Inventory()
            {
                AIMS.Data.ProductData proData = new ProductData(config);
                OrderData orData = new OrderData(config);
                OrderController orControl = new OrderController(orData, proData);
                InventoryController newInventController = new InventoryController(proData, (IHttpContextAccessor)MockHttpContext());
                int newProductID = ((newInventController.Index() as ViewResult).ViewData.Model as List<Product>).Last().ProductId;
                AIMS.Models.Orders orders = new AIMS.Models.Orders
                {
                    CreatedAt = DateTime.Now,
                    OrderQuantity = 2,
                    ProductId = newProductID,
                };
                var result = orControl.UpdateInventory() as RedirectToActionResult;
                Assert.AreEqual("Index", result.ActionName);
                Assert.AreEqual("Inventory", result.ControllerName);
            }

            [Test]
            public void IActionUpdateInventory_OrderController_OnlyIndex()
            {
                AIMS.Data.ProductData proData = new ProductData(config);
                OrderData orData = new OrderData(config);
                OrderController orControl = new OrderController(orData, proData);
                
                AIMS.Models.Product product = new AIMS.Models.Product
                {
                    Branch = "London",
                    Category = "Accessories",
                    Price = 20,
                    ProductDescription = "cool looking clothing",
                    ProductName = "Nice hat",
                    ProductSize = "ONE SIZE",
                    Quantity = 0,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };
                InventoryController productController = new InventoryController(proData, (IHttpContextAccessor)MockHttpContext());
                productController.Create(product);
                List<Product> newList = (productController.Index() as ViewResult).ViewData.Model as List<Product>;
                Product centralProduct = newList.Last();

                //This test was particularly difficult due to my inexperience with mocking systems.
                //Here, we are trying to get the UpdateInventory method for the Order Controller class to
                //return a RedirectToActionResult that has the ActionName as "Input" but DOESN'T HAVE the ControllerName as "Inventory"
                //and I had a lot of trouble emulating the system
                //eventually I ran out of time to work on it, unfortunately

                productController.Edit(product, centralProduct.ProductId);

                AIMS.Models.Orders orders = new AIMS.Models.Orders
                {
                    CreatedAt = DateTime.Now,
                    OrderQuantity = 2,
                    ProductId = centralProduct.ProductId,
                };

                orControl.AddItem(orders);
                

                var result = orControl.UpdateInventory() as RedirectToActionResult;
                Assert.AreEqual("Index", result.ActionName);
                Assert.AreNotEqual("Inventory", result.ControllerName);
            }

        }

        public class ProductController_Test
        {
            [Test]
            public void IActionIndexTest_ProductController()
            {
                AIMS.Data.ProductData proData = new ProductData(config);
                ProductController proControl = new ProductController(proData);
                var result = proControl.Index() as ViewResult;
                List<AIMS.Models.Product> newList = (List<AIMS.Models.Product>)result.Model;
                

                var resultData = result.ViewData.Model;
                var modelType = resultData.GetType().ToString();
                Assert.That(modelType.Contains("AIMS.Models.Product"));
            }
        }

        public class UserController_Test
        {
            //Index
            [Test]
            public void IActionIndexTest_UserController()
            {
                UserData uData = new UserData(config);
                UserController userController = new UserController(uData, (IHttpContextAccessor)MockHttpContext());
                var result = userController.Index() as ViewResult;
                var resultX = result.ViewData.Model;
                var resultData = resultX.GetType().ToString();

                //"System.Collections.Generic.List`1[AIMS.Models.Users]"
                Assert.That(resultData.Contains("AIMS.Models.Users"));
                
            }

            //Create
            [Test]
            public void IActionCreateTest_UserController_Empty()
            {
                UserData uData = new UserData(config);
                UserController userController = new UserController(uData, (IHttpContextAccessor)MockHttpContext());
                var result = userController.Create() as ViewResult;
                var resultData = result.ViewData.Model;
                Assert.IsNull(resultData);
                
            }

            [Test]
            public void IActionCreateTest_UserController_Parameters_Index()
            {
                UserData uData = new UserData(config);
                UserController userController = new UserController(uData, (IHttpContextAccessor)MockHttpContext());
                Users user = new Users
                {
                    Branch = "London",
                    FirstName = "Jessie",
                    LastName = "Bell",
                    Role = "Admin",
                    CreatedDate = DateTime.Now,
                    Email = "jessieBell@pokemon.com",
                    UserPassword = "jessiesbetterthanjames"
                };
                var result = userController.Create(user) as RedirectToActionResult;
                Assert.AreEqual("Index", result.ActionName);
            }

            [Test]
            public void IActionCreateTest_UserController_Parameters_User()
            {
                UserData uData = new UserData(config);
                UserController userController = new UserController(uData, (IHttpContextAccessor)MockHttpContext());
                Users user = new Users
                {
                    Branch = "London",
                    FirstName = "Jessie",
                    LastName = "Bell",
                    Role = "Admin",
                    CreatedDate = DateTime.Now,
                    Email = "jessieBell@pokemon.com",
                    UserPassword = "jessiesbetterthanjames"
                };

                Object final = false;
                userController.ModelState.SetModelValue("IsValid", final, "false");

                var result = userController.Create(user) as ViewResult;
                var resultData = result.ViewData.Model.GetType().ToString();
                Assert.AreEqual(resultData, "AIMS.Models.Users");
            }

            //Update
            [Test]
            public void IActionEditTest_UserController_Int()
            {
                UserData uData = new UserData(config);
                UserController userController = new UserController(uData, (IHttpContextAccessor)MockHttpContext());
                List<Users> userList = (userController.Index() as ViewResult).ViewData.Model as List<Users>;
                int lastUserID = userList.Last().UserId;
                var result = userController.Edit(lastUserID) as ViewResult;
                var resultData = result.ViewData.Model.GetType().ToString();
                Assert.AreEqual(resultData, "AIMS.Models.Users");
            }

            [Test]
            public void IActionEditTest_UserController_Parameters_Index()
            {
                UserData uData = new UserData(config);
                UserController userController = new UserController(uData, (IHttpContextAccessor)MockHttpContext());
                Users user = new Users
                {
                    Branch = "London",
                    FirstName = "Jessie",
                    LastName = "Bell",
                    Role = "Admin",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    Email = "jessieBell@pokemon.com",
                    UserPassword = "jessiesbetterthanjames"
                };
                userController.Create(user);
                int lastUserId = ((userController.Index() as ViewResult).ViewData.Model as List<Users>).Last().UserId;
                var result = userController.Edit(user, lastUserId) as RedirectToActionResult;
                Assert.AreEqual("Index", result.ActionName);
            }

            [Test]
            public void IActionEditTest_UserController_Parameters_User()
            {
                UserData uData = new UserData(config);
                UserController userController = new UserController(uData, (IHttpContextAccessor)MockHttpContext());
                Users user = new Users
                {
                    Branch = "London",
                    FirstName = "Jessie",
                    LastName = "Bell",
                    Role = "Admin",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    Email = "jessieBell@pokemon.com",
                    UserPassword = "jessiesbetterthanjames"
                };

                userController.Create(user);
                int lastUserId = ((userController.Index() as ViewResult).ViewData.Model as List<Users>).Last().UserId;

                Object final = false;
                userController.ModelState.SetModelValue("IsValid", final, "false");

                var result = userController.Edit(user, lastUserId) as ViewResult;
                var resultData = result.ViewData.ModelState.IsValid;
                var resultModelData = result.ViewData.ModelState.Count;
                Assert.IsFalse(resultData);
                Assert.That(resultModelData > 0);
            }

            [Test]
            public void IActionEditTest_UserController_Parameters_ModelError()
            {
                DateTime dateTime = DateTime.Parse("04/01/2025 00:00:00");
                UserData uData = new UserData(config);
                UserController userController = new UserController(uData, (IHttpContextAccessor)MockHttpContext());
                Users user = new Users
                {
                    Branch = "London",
                    FirstName = "Jessie",
                    LastName = "Bell",
                    Role = "Admin",
                    CreatedDate = dateTime,
                    UpdatedDate = dateTime,
                    Email = "jessieBell@pokemon.com",
                    UserPassword = "jessiesbetterthanjames"
                };

                Users user2 = new Users
                {
                    Branch = "Sheffield",
                    FirstName = "James",
                    LastName = "Charles",
                    Role = "Admin",
                    CreatedDate = dateTime,
                    UpdatedDate = dateTime,
                    Email = "jamescharles@pokemon.com",
                    UserPassword = "jameslovesgrowlithe"
                };

                userController.Create(user);
                userController.Create(user2); 
                
                int lastUserID = ((userController.Index() as ViewResult).ViewData.Model as List<Users>).Last().UserId;

                Object final = true;
                userController.ModelState.SetModelValue("IsValid", final, "true");

                var result = userController.Edit(user, lastUserID) as ViewResult;
                var resultData = result.ViewData.ModelState.Count;
                Assert.That(resultData > 0);
            }

            //Delete
            [Test]
            public void IActionDeleteTest_UserController_Empty()
            {
                UserData uData = new UserData(config);
                UserController userController = new UserController(uData, (IHttpContextAccessor)MockHttpContext());
                Users user = new Users
                {
                    Branch = "London",
                    FirstName = "Jessie",
                    LastName = "Bell",
                    Role = "Admin",
                    CreatedDate = DateTime.Now,
                    Email = "jessieBell@pokemon.com",
                    UserPassword = "jessiesbetterthanjames"
                };
                var createResult = userController.Create(user) as RedirectToActionResult;
                

                var result = userController.Delete() as ViewResult;
                var resultData = result.ViewData.Model;
                Assert.IsNull(resultData);
            }

            [Test]
            public void IActionDeleteTest_UserController_Int()
            {
                UserData uData = new UserData(config);
                UserController userController = new UserController(uData, (IHttpContextAccessor)MockHttpContext());
                Users user = new Users
                {
                    Branch = "London",
                    FirstName = "Jessie",
                    LastName = "Bell",
                    Role = "Admin",
                    CreatedDate = DateTime.Now,
                    Email = "jessieBell@pokemon.com",
                    UserPassword = "jessiesbetterthanjames"
                };
                
                userController.Create(user);
                int lastUserId = ((userController.Index() as ViewResult).ViewData.Model as List<Users>).Last().UserId;
                

                var result = userController.Delete(lastUserId) as RedirectToActionResult;
                Assert.AreEqual("Index", result.ActionName);
            }
        }

        public class HistoryController_Test
        {
            [Test]
            public void IActionIndexTest_HistoryController()
            {
                HistoryData hisData = new HistoryData(config);
                IHContextAcc(MockHttpContext());
                //IHttpContextAccessor hey = new IHttpContextAccessor(MockHttpContext());
                HistoryController hisController = new HistoryController(hisData, IHContextAcc(MockHttpContext()));
                var result = hisController.Index() as ViewResult;
                string resultValue = result.ViewData.Model.GetType().ToString();
                Assert.That(resultValue.Contains("OrderDetails"));
            }


        }

        public class LoginController_Test
        {
            //Index
            [Test]
            public void IActionIndexTest_LoginController_Empty()
            {
                UserService uServ = new UserService(config);
                LoginController logCon = new LoginController(uServ);
                var result = logCon.Index() as ViewResult;
                Assert.IsNotNull(result);
            }

            [Test]
            public void IActionIndexTest_LoginController_Parameters_View()
            {
                UserService uServ = new UserService(config);
                LoginController logCon = new LoginController(uServ);
                var result = logCon.Index("myemail@hello.com", "mypasswordlol") as ViewResult;
                Assert.IsNotNull(result);
            }

            [Test]
            public void IActionIndexTest_LoginController_Parameters_Index()
            {
                UserService uServ = new UserService(config);
                LoginController logCon = new LoginController(uServ);
                var result = logCon.Index("myemail@hello.com", "mypasswordlol") as ViewResult;
                Assert.IsNotNull(result);
            }

            //Logout
            [Test] //does not work
            public void IActionLogOutTest_LoginController()
            {
                Object sessionControl = new object();
                
                UserService uServ = new UserService(config);
                LoginController logCon = new LoginController(uServ);
                logCon.ControllerContext = new ControllerContext();
                logCon.ControllerContext.HttpContext = new DefaultHttpContext();
                logCon.ControllerContext.HttpContext.Session.SetString("mystring", "myotherstring");
                var result = logCon.Logout() as RedirectToActionResult;
                Assert.AreEqual(result.ActionName, "Index");
            }

        }
    }
}