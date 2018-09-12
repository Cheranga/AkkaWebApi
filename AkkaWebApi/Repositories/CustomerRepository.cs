using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AkkaWebApi.Models;
using Dapper;

namespace AkkaWebApi.Repositories
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetCustomerByNameAsync(string name);
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Customer>> GetCustomerByNameAsync(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var customers = await connection.QueryAsync<Customer>("SELECT * FROM CUSTOMER WHERE FIRSTNAME LIKE @FirstName", new {FirstName = name});

                return (customers?.ToList()) ?? new List<Customer>();
            }
        }
    }
}
