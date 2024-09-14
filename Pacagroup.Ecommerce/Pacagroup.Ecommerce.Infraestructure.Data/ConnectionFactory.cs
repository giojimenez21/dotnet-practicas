using Microsoft.Extensions.Configuration;
using Pacagroup.Ecommerce.Transversal.Common;
using System.Data;
using System.Data.SqlClient;

namespace Pacagroup.Ecommerce.Infraestructure.Data
{
    public class ConnectionFactory(IConfiguration configuration) : IConnectionFactory
    {
        private readonly IConfiguration configuration = configuration;

        public IDbConnection GetConnection
        {
            get
            {
                var sqlConnection = new SqlConnection();
                if (sqlConnection is null) return null;
                sqlConnection.ConnectionString = configuration.GetConnectionString("DbConnection");
                sqlConnection.Open();
                return sqlConnection;
            }
        }
    }
}
