﻿using AIMS.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;

namespace AIMS.Data
{
    public class OrderData : DataAccess
    {
        public OrderData(IConfiguration configuration) : base(configuration){ }

		//Join Orders and Product tables to get product details
		public List<OrderDetails> GetProductDetail()
        {
            List<OrderDetails> orderDetailList = new List<OrderDetails>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "SELECT Orders.OrderId, Orders.OrderQuantity, Product.ProductId, Product.ProductName, Product.ProductSize, Product.Price FROM Product INNER JOIN Orders ON Product.ProductId = Orders.ProductId";
                SqlCommand command = new SqlCommand(sql, connection);

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
						OrderDetails orderDetail = new OrderDetails();
						orderDetail.OrderId = Convert.ToInt32(dataReader["OrderId"]);
						orderDetail.OrderQuantity = Convert.ToInt32(dataReader["OrderQuantity"]);
						orderDetail.ProductId = Convert.ToInt32(dataReader["ProductId"]);
						orderDetail.ProductName = Convert.ToString(dataReader["ProductName"]);
						orderDetail.ProductSize = Convert.ToString(dataReader["ProductSize"]);
						orderDetail.Price = Convert.ToDecimal(dataReader["Price"]);

						orderDetailList.Add(orderDetail);
					}
                }
            }
            return orderDetailList;
        }

		//Create new order and insert into DB
		public void AddItemToOrder(Orders orders)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				string sql = @"
						IF NOT EXISTS 
							(SELECT * FROM Orders WHERE ProductId=@ProductId) 
							BEGIN INSERT INTO Orders (ProductId, OrderQuantity) 
							VALUES(@ProductId, @OrderQuantity) 
							END 
						ELSE 
							BEGIN UPDATE Orders SET OrderQuantity=OrderQuantity + 1 
							WHERE ProductId=@ProductId 
							END ";
				SqlCommand command = new SqlCommand(sql, connection);
				command.Parameters.AddWithValue("@ProductId", orders.ProductId);
				command.Parameters.AddWithValue("@OrderQuantity", 1);

				connection.Open();
				command.ExecuteNonQuery();
			}
		}

		//Update item qauntity in DB
		public void UpdateOrder(Orders order, int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
				string sql = "UPDATE Orders SET OrderQuantity = @OrderQuantity WHERE OrderId = @OrderId";
				SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@OrderId", id);
                command.Parameters.AddWithValue("@OrderQuantity", order.OrderQuantity);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        //Delete product from DB
        public void RemoveItemFromOrder(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "DELETE FROM Orders WHERE OrderId = @OrderId";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@OrderId", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

		//Get single product using ID from DB
		public Orders GetOrderById(int orderId)
		{
			Orders order = null;

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				string sql = "SELECT * FROM Orders WHERE OrderId=@OrderId";
				SqlCommand command = new SqlCommand(sql, connection);
				command.Parameters.AddWithValue("@OrderId", orderId);

				connection.Open();

				using (SqlDataReader dataReader = command.ExecuteReader())
				{
					if (dataReader.Read())
					{
						order = new Orders
						{
							OrderId = (int)dataReader["OrderId"],
							OrderQuantity = (int)dataReader["OrderQuantity"],
							CreatedAt = (DateTime)dataReader["CreatedAt"],
							ProductId = (int)dataReader["ProductId"]
						};
					}
				}
			}
			return order;
		}

		//update inventory quantity
		public void UpdateInventoryQuantity()
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				string sql = "UPDATE Product SET Product.Quantity = (Product.Quantity - Orders.OrderQuantity) FROM Product INNER JOIN Orders ON Product.ProductId = Orders.ProductId WHERE Product.ProductId = Orders.ProductId";
				SqlCommand command = new SqlCommand(sql, connection);
	
				connection.Open();
				command.ExecuteNonQuery();
			}
		}

		//Archive orders
		public void ArchivedOrderDetails()
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				// Generate a 4-digit number
				Random random = new Random();
				int orderNumber = random.Next(1000, 10000);

				// Copy data to ArchivedOrderDetails
				string copyQuery = @"
                    INSERT INTO ArchivedOrderDetails (OrderId , OrderQuantity, ProductId, OrderNumber)
                    SELECT OrderId, OrderQuantity, ProductId, @OrderNumber
                    FROM Orders";

				using (SqlCommand copyCommand = new SqlCommand(copyQuery, connection))
				{
					copyCommand.Parameters.AddWithValue("@OrderNumber", orderNumber);
					copyCommand.ExecuteNonQuery();
				}

				// Delete data from Orders and not the table itself
				string deleteQuery = "TRUNCATE TABLE Orders";

				using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
				{
					deleteCommand.ExecuteNonQuery();
				}
			}
		}
	}
}
