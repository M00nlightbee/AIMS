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
                string sql = "SELECT Orders.OrderId, Product.ProductId, Product.ProductName, Product.ProductSize, Product.Price FROM Product INNER JOIN Orders ON Product.ProductId = Orders.ProductId";
                SqlCommand command = new SqlCommand(sql, connection);

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        OrderDetails orderDetail = new OrderDetails();
                        orderDetail.OrderId = Convert.ToInt32(dataReader["OrderId"]);
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

        //Update product information and update DB
        //public void UpdateOrder(Orders order, int id)
        //{
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        string sql = "UPDATE Orders SET OrderQuantity = @OrderQuantity, ItemTotalPrice = @ItemTotalPrice, ProductId = @ProductId WHERE OrderId = @OrderId";
        //        SqlCommand command = new SqlCommand(sql, connection);
        //        command.Parameters.AddWithValue("@OrderId", id);
        //        //command.Parameters.AddWithValue("@OrderNo", order.OrderNo);
        //        command.Parameters.AddWithValue("@OrderQuantity", order.OrderQuantity);
        //        command.Parameters.AddWithValue("@@ItemTotalPrice * @OrderQuantity", order.ItemTotalPrice);
        //        //command.Parameters.AddWithValue("@CreatedAt", order.CreatedAt);
        //        command.Parameters.AddWithValue("@ProductId", order.ProductId);

        //        connection.Open();
        //        command.ExecuteNonQuery();
        //    }
        //}

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
        //    public Orders GetOrderById(int orderId)
        //    {
        //        Orders order = null;

        //        using (SqlConnection connection = new SqlConnection(_connectionString))
        //        {
        ////string sql = "SELECT * FROM Orders WHERE OrderId=@OrderId";
        //string sql = "SELECT * FROM Orders JOIN Product ON Product.ProductId = Orders.ProductId WHERE OrderId=@OrderId";
        //SqlCommand command = new SqlCommand(sql, connection);
        //            command.Parameters.AddWithValue("@OrderId", orderId);

        //            connection.Open();

        //            using (SqlDataReader dataReader = command.ExecuteReader())
        //            {
        //                if (dataReader.Read())
        //                {
        //                    order = new Orders
        //                    {
        //                        OrderId = (int)dataReader["OrderId"],
        //                        OrderNo = (string)dataReader["OrderNo"],
        //                        //ItemName = (string)dataReader["ItemName"],
        //                        OrderQuantity = (int)dataReader["OrderQuantity"],
        //                        //ItemSize = (string)dataReader["ItemSize"],
        //                        //ItemUnitPrice = (decimal)dataReader["ItemUnitPrice"],
        //                        ItemTotalPrice = (decimal)dataReader["ItemTotalPrice"],
        //                        CreatedAt = (string)dataReader["CreatedAt"],
        //                        ProductId = (int)dataReader["ProductId"]

        //                    };
        //                }
        //            }
        //        }
        //        return order;
        //    }
    }
}
