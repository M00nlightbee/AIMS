using AIMS.Models;
using Microsoft.Data.SqlClient;

namespace AIMS.Data
{
    public class UserData : DataAccess
    {
        public UserData(IConfiguration configuration) : base(configuration)
        {
        }

        // Get product details from DB
        public List<Users> GetUsers()
        {
            List<Users> userList = new List<Users>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Users";
                SqlCommand command = new SqlCommand(sql, connection);

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Users user = new Users();
                        user.UserId = Convert.ToInt32(dataReader["UserId"]);
                        user.FirstName = Convert.ToString(dataReader["FirstName"]);
                        user.LastName = Convert.ToString(dataReader["LastName"]);
                        user.Branch = Convert.ToString(dataReader["Branch"]);
                        user.Role = Convert.ToString(dataReader["Role"]);
                        user.Email = Convert.ToString(dataReader["Email"]);
                        user.UserPassword = Convert.ToString(dataReader["UserPassword"]);
                        user.CreatedDate = Convert.ToDateTime(dataReader["CreatedDate"]);
                        user.UpdatedDate = Convert.ToDateTime(dataReader["UpdatedDate"]);

                        userList.Add(user);
                    }
                }
            }
            return userList;
        }

        //Create new user and insert into DB
        public void CreateUser(Users user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "INSERT INTO Users (FirstName, LastName, Branch, Role, Email, UserPassword) VALUES (@FirstName, @LastName, @Branch, @Role, @Email, @UserPassword)";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@Branch", user.Branch);
                command.Parameters.AddWithValue("@Role", user.Role);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@UserPassword", user.UserPassword);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        //Update user information and update DB
        public void UpdateUser(Users user, int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "UPDATE Users SET FirstName = @FirstName, LastName = @LastName, Branch = @Branch, Role = @Role, Email = @Email, UserPassword = @UserPassword, UpdatedDate = @UpdatedDate WHERE UserId = @UserId";
                SqlCommand command = new SqlCommand(sql, connection);
				command.Parameters.AddWithValue("@UserId", id);
				command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@Branch", user.Branch);
                command.Parameters.AddWithValue("@Role", user.Role);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@UserPassword", user.UserPassword);
                command.Parameters.AddWithValue("@UpdatedDate", user.UpdatedDate);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        //Delete user from DB
        public void DeleteUser(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "DELETE FROM Users WHERE UserId = @UserId";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@UserId", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        //Get single product using ID from DB
        public Users? GetUserById(int userId)
        {
            Users? user = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Users WHERE UserId=@UserId";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        user = new Users
                        {
                            UserId = (int)dataReader["UserId"],
                            FirstName = (string)dataReader["FirstName"],
                            LastName = (string)dataReader["LastName"],
                            Branch = (string)dataReader["Branch"],
                            Role = (string)dataReader["Role"],
                            Email = (string)dataReader["Email"],
                            UserPassword = (string)dataReader["UserPassword"],
                            UpdatedDate = (DateTime)dataReader["UpdatedDate"]
                        };
                    }
                }
            }
            return user;
        }
    }
}
