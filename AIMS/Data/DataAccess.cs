using Microsoft.Extensions.Configuration;

namespace AIMS.Data
{
    // Access connection string from appsetting.json
    public class DataAccess
    {
        protected readonly string _connectionString;
        public DataAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string cannot be found");
        }
    }
}
