using System.Data;
using System.Data.SqlClient;

namespace Practica2_API.Data
{
    public interface IConnectionProvider
    {
        IDbConnection GetConnection();
    }

    public class SqlServerConnectionProvider : IConnectionProvider
    {
        private readonly string _connectionString;

        public SqlServerConnectionProvider(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public IDbConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
