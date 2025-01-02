using AIMS.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;

namespace AIMS.Data
{
    public class OrderData : DataAccess
    {
        public OrderData(IConfiguration configuration) : base(configuration){ }

        //Create new order and insert into DB
        public List<OrderDetails> GetProductDetail()
        {
            List<OrderDetails> orderDetailList = new List<OrderDetails>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "SELECT Orders.OrderId, Orders.OrderQuantity, Orders.ItemTotalPrice, Product.ProductId, Product.ProductName, Product.ProductSize, Product.Price FROM Product INNER JOIN Orders ON Product.ProductId = Orders.ProductId";
                SqlCommand command = new SqlCommand(sql, connection);

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        //OrderDetails orderDetail = new OrderDetails();
                        //orderDetail.OrderId = Convert.ToInt32(dataReader["OrderId"]);
                        //orderDetail.OrderQuantity = Convert.ToInt32(dataReader["OrderQuantity"]);
                        //orderDetail.ItemTotalPrice = Convert.ToDecimal(dataReader["ItemTotalPrice"]);
                        //orderDetail.ProductId = Convert.ToInt32(dataReader["ProductId"]);
                        //orderDetail.ProductName = Convert.ToString(dataReader["ProductName"]);
                        //orderDetail.ProductSize = Convert.ToString(dataReader["ProductSize"]);
                        //orderDetail.Price = Convert.ToDecimal(dataReader["Price"]);

                        //orderDetailList.Add(orderDetail);

                        OrderDetails orderDetail = new OrderDetails();
						orderDetail.OrderId = Convert.ToInt32(dataReader["OrderId"]);
						orderDetail.OrderQuantity = dataReader["OrderQuantity"] != DBNull.Value ? Convert.ToInt32(dataReader["OrderQuantity"]) : 1;
						orderDetail.ItemTotalPrice = dataReader["ItemTotalPrice"] != DBNull.Value ? Convert.ToDecimal(dataReader["ItemTotalPrice"]) : 0;
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
                string sql = "INSERT INTO Orders (ProductId) VALUES (@ProductId)";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@ProductId", orders.ProductId);

				connection.Open();
                command.ExecuteNonQuery();
            }
        }

		//Update item qauntity in DB
		public void UpdateOrder(Orders order, int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
				//string sql = "UPDATE Orders SET OrderQuantity = @OrderQuantity, ItemTotalPrice = @ItemTotalPrice WHERE OrderId = @OrderId";
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
		//public Orders GetOrderById(int orderId)
		//{
		//    Orders order = null;

		//    using (SqlConnection connection = new SqlConnection(_connectionString))
		//    {
		//        string sql = "SELECT * FROM Orders WHERE OrderId=@OrderId";
		//        SqlCommand command = new SqlCommand(sql, connection);
		//        command.Parameters.AddWithValue("@OrderId", orderId);

		//        connection.Open();

		//        using (SqlDataReader dataReader = command.ExecuteReader())
		//        {
		//            if (dataReader.Read())
		//            {
		//                order = new Orders
		//                {
		//                    OrderId = (int)dataReader["OrderId"],
		//                    OrderQuantity = (int)dataReader["OrderQuantity"],
		//                    ItemTotalPrice = (decimal)dataReader["ItemTotalPrice"],
		//                    CreatedAt = (DateTime)dataReader["CreatedAt"],
		//                    ProductId = (int)dataReader["ProductId"]
		//                };
		//            }
		//        }
		//    }
		//    return order;
		//}

		public Orders GetOrderById(int orderId)
		{
			Orders order = null;

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				string sql = "SELECT OrderQuantity FROM Orders WHERE OrderId=@OrderId";
				SqlCommand command = new SqlCommand(sql, connection);
				command.Parameters.AddWithValue("@OrderId", orderId);

				connection.Open();

				using (SqlDataReader dataReader = command.ExecuteReader())
				{
					if (dataReader.Read())
					{
						order = new Orders
						{
							OrderQuantity = dataReader["OrderQuantity"] != DBNull.Value ? (int)dataReader["OrderQuantity"] : 1,
						};
					}
				}
			}
			return order;
		}
	}
}
