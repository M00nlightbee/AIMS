using AIMS.Data;
using AIMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AIMS.Services
{
    public class UserService : DataAccess
    {
        public UserService(IConfiguration configuration) : base(configuration)
		{
        }

        public Users ValidateUser(string email, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = @"
                    SELECT UserId, FirstName, LastName, Email, Role 
                    FROM Users 
                    WHERE Email = @Email AND UserPassword = @Password";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Users
                            {
                                UserId = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Email = reader.GetString(3),
                                Role = reader.GetString(4)
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}
