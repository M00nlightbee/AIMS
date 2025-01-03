using AIMS.Models;
using Microsoft.Data.SqlClient;

namespace AIMS.Data
{
	public class HistoryData : DataAccess
	{
		public HistoryData(IConfiguration configuration) : base(configuration)
		{
		}

		//Get all order history joing product and order details
		public List<OrderDetails> GetOrderHistory()
		{
			List<OrderDetails> orderDetailList = new List<OrderDetails>();

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				string sql = @"SELECT 
								ArchivedOrderDetails.OrderNumber, 
								ArchivedOrderDetails.OrderQuantity,
								ArchivedOrderDetails.CreatedAt,
								Product.ProductId, 
								Product.ProductName, 
								Product.ProductSize, 
								Product.Price 
							    FROM Product 
							    INNER JOIN ArchivedOrderDetails 
							    ON Product.ProductId = ArchivedOrderDetails.ProductId";
				SqlCommand command = new SqlCommand(sql, connection);

				using (SqlDataReader dataReader = command.ExecuteReader())
				{
					while (dataReader.Read())
					{
						OrderDetails orderDetail = new OrderDetails();
						orderDetail.OrderNumber = Convert.ToInt32(dataReader["OrderNumber"]);
						orderDetail.OrderQuantity = Convert.ToInt32(dataReader["OrderQuantity"]);
						orderDetail.ProductId = Convert.ToInt32(dataReader["ProductId"]);
						orderDetail.ProductName = Convert.ToString(dataReader["ProductName"]);
						orderDetail.ProductSize = Convert.ToString(dataReader["ProductSize"]);
						orderDetail.Price = Convert.ToDecimal(dataReader["Price"]);
						orderDetail.CreatedAt = Convert.ToDateTime(dataReader["CreatedAt"]);

						orderDetailList.Add(orderDetail);
					}
				}
			}
			return orderDetailList;
		}
	}
}
