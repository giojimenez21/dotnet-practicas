using Dapper;
using Pacagroup.Ecommerce.Domain.Entity;
using Pacagroup.Ecommerce.Infraestructure.Interface;
using Pacagroup.Ecommerce.Transversal.Common;
using System.Data;

namespace Pacagroup.Ecommerce.Infraestructure.Repository
{
    public class CustomerRepository(IConnectionFactory connectionFactory) : ICustomersRepository
    {
        private readonly IConnectionFactory connectionFactory = connectionFactory;

        public bool Delete(string customerId)
        {
            using (var connection = connectionFactory.GetConnection)
            {
                var query = "CustomersDelete";
                var parameters = new DynamicParameters();
                parameters.Add("CustomerID", customerId);
                var result = connection.Execute(query, parameters, commandType: CommandType.StoredProcedure);
                return result > 0;
            }
        }

        public async Task<bool> DeleteAsync(string customerId)
        {
            using (var connection = connectionFactory.GetConnection)
            {
                var query = "CustomersDelete";
                var parameters = new DynamicParameters();
                parameters.Add("CustomerID", customerId);
                var result = await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
                return result > 0;
            }
        }

        public IEnumerable<Customers> GetAll()
        {
            using (var connection = connectionFactory.GetConnection)
            {
                var query = "CustomersList";
                var result = connection.Query<Customers>(query, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<IEnumerable<Customers>> GetAllAsync()
        {
            using (var connection = connectionFactory.GetConnection)
            {
                var query = "CustomersList";
                var result = await connection.QueryAsync<Customers>(query, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public Customers GetById(string customerId)
        {
            using (var connection = connectionFactory.GetConnection)
            {
                var query = "CustomersGetById";
                var parameters = new DynamicParameters();
                parameters.Add("CustomerID", customerId);
                var result = connection.QuerySingle<Customers>(query, parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<Customers> GetByIdAsync(string customerId)
        {
            using (var connection = connectionFactory.GetConnection)
            {
                var query = "CustomersGetById";
                var parameters = new DynamicParameters();
                parameters.Add("CustomerID", customerId);
                var result = await connection.QuerySingleAsync<Customers>(
                    query,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result;
            }
        }

        public bool Insert(Customers customer)
        {
            using (var connection = connectionFactory.GetConnection)
            {
                var query = "CustomersInsert";
                var parameters = new DynamicParameters();
                parameters.Add("CustomerID", customer.CustomerId);
                var result = connection.Execute(query, parameters, commandType: CommandType.StoredProcedure);
                return result > 0;
            }
        }

        public async Task<bool> InsertAsync(Customers customer)
        {
            using (var connection = connectionFactory.GetConnection)
            {
                var query = "CustomersInsert";
                var parameters = new DynamicParameters();
                parameters.Add("CustomerID", customer.CustomerId);
                var result = await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
                return result > 0;
            }
        }

        public bool Update(Customers customer)
        {
            using (var connection = connectionFactory.GetConnection)
            {
                var query = "CustomersUpdate";
                var parameters = new DynamicParameters();
                parameters.Add("CustomerID", customer.CustomerId);
                var result = connection.Execute(query, parameters, commandType: CommandType.StoredProcedure);
                return result > 0;
            }
        }

        public async Task<bool> UpdateAsync(Customers customer)
        {
            using (var connection = connectionFactory.GetConnection)
            {
                var query = "CustomersUpdate";
                var parameters = new DynamicParameters();
                parameters.Add("CustomerID", customer.CustomerId);
                var result = await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
                return result > 0;
            }
        }
    }
}
