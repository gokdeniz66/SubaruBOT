using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace SubaruBOT.Data
{
    public class Database
    {
        private readonly string _connectionString;

        public Database(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection not found");
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public string ConnectionString => _connectionString;
    }
}