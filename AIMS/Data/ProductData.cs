
//Dependency
using System.Data;
using AIMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

//CRUD for the product model
namespace AIMS.Data
{
	// confirguration to access connection string from appsetttings.json
    public class ProductData : DataAccess
    {
        public ProductData(IConfiguration configuration) : base(configuration)
        {
        }

		// Get product details from DB
		public List<Product> GetProducts()
		{
			List<Product> productList = new List<Product>();

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				string sql = "SELECT * FROM Product";
				SqlCommand command = new SqlCommand(sql, connection);

				using (SqlDataReader dataReader = command.ExecuteReader())
				{
					while (dataReader.Read())
					{
                        Product product = new Product();
						product.ProductId = Convert.ToInt32(dataReader["ProductId"]);
						product.ProductName = Convert.ToString(dataReader["ProductName"]);
						product.ProductDescription = Convert.ToString(dataReader["ProductDescription"]);
						product.ProductSize = Convert.ToString(dataReader["ProductSize"]);
						product.Quantity = Convert.ToInt32(dataReader["Quantity"]);
						product.Price = Convert.ToDecimal(dataReader["Price"]);
						product.Branch = Convert.ToString(dataReader["Branch"]);
						product.Category = Convert.ToString(dataReader["Category"]);
						product.CreatedDate = Convert.ToDateTime(dataReader["CreatedDate"]);
						product.UpdatedDate = Convert.ToDateTime(dataReader["UpdatedDate"]);

						productList.Add(product);
					}
				}
			}
			return productList;
		}

		//Create new product and insert into DB
		public void CreateProduct(Product product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "INSERT INTO Product (ProductName, ProductDescription, ProductSize, Quantity, Price, Branch, Category) VALUES (@ProductName, @ProductDescription, @ProductSize, @Quantity, @Price, @Branch, @Category)";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@ProductDescription", product.ProductDescription);
                command.Parameters.AddWithValue("@ProductSize", product.ProductSize);
                command.Parameters.AddWithValue("@Quantity", product.Quantity);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Branch", product.Branch);
                command.Parameters.AddWithValue("@Category", product.Category);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

		//Update product information and update DB
		public void UpdateProduct(Product product, int id)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				string sql = "UPDATE Product SET ProductName = @ProductName, ProductDescription = @ProductDescription, ProductSize = @ProductSize, Quantity = @Quantity, Price = @Price, Branch = @Branch, Category = @Category, UpdatedDate = @UpdatedDate WHERE ProductId = @ProductId";
				SqlCommand command = new SqlCommand(sql, connection);
				command.Parameters.AddWithValue("@ProductId", id);
				command.Parameters.AddWithValue("@ProductName", product.ProductName);
				command.Parameters.AddWithValue("@ProductDescription", product.ProductDescription);
				command.Parameters.AddWithValue("@ProductSize", product.ProductSize);
				command.Parameters.AddWithValue("@Quantity", product.Quantity);
				command.Parameters.AddWithValue("@Price", product.Price);
				command.Parameters.AddWithValue("@Branch", product.Branch);
				command.Parameters.AddWithValue("@Category", product.Category);
				command.Parameters.AddWithValue("@UpdatedDate", product.UpdatedDate);

				connection.Open();
				command.ExecuteNonQuery();
			}
		}

		//Delete product from DB
		public void DeleteProduct(int id)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				string sql = "DELETE FROM Product WHERE ProductId = @ProductId";
				SqlCommand command = new SqlCommand(sql, connection);
				command.Parameters.AddWithValue("@ProductId", id);

				connection.Open();
				command.ExecuteNonQuery();
			}
		}

		//Get single product using ID from DB
		public Product? GetProductById(int productId)
		{
			Product? product = null;

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				string sql = "SELECT * FROM Product WHERE ProductId=@ProductId";
				SqlCommand command = new SqlCommand(sql, connection);
				command.Parameters.AddWithValue("@ProductId", productId);

				connection.Open();

				using (SqlDataReader dataReader = command.ExecuteReader())
				{
					if (dataReader.Read())
					{
						product = new Product
						{
							ProductId = (int)dataReader["ProductId"],
							ProductName = (string)dataReader["ProductName"],
							ProductDescription = (string)dataReader["ProductDescription"],
							ProductSize = (string)dataReader["ProductSize"],
							Quantity = (int)dataReader["Quantity"],
							Price = (decimal)dataReader["Price"],
							Branch = (string)dataReader["Branch"],
							Category = (string)dataReader["Category"],
							CreatedDate = (DateTime)dataReader["CreatedDate"],
							UpdatedDate = (DateTime)dataReader["UpdatedDate"]
						};
					}
				}
			}
			return product;
		}
    }
}
