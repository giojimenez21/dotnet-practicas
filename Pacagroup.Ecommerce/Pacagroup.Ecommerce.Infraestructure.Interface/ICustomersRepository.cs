using Pacagroup.Ecommerce.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacagroup.Ecommerce.Infraestructure.Interface
{
    public interface ICustomersRepository
    {
        public bool Insert(Customers customer);
        public bool Update(Customers customer);
        public bool Delete(string customerId);
        public Customers GetById(string customerId);
        public IEnumerable<Customers> GetAll();

        public Task<bool> InsertAsync(Customers customer);
        public Task<bool> UpdateAsync(Customers customer);
        public Task<bool> DeleteAsync(string customerId);
        public Task<Customers> GetByIdAsync(string customerId);
        public Task<IEnumerable<Customers>> GetAllAsync();

    }
}
