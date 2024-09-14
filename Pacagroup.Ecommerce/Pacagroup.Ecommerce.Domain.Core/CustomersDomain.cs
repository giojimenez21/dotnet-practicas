using Pacagroup.Ecommer.Domain.Interface;
using Pacagroup.Ecommerce.Domain.Entity;
using Pacagroup.Ecommerce.Infraestructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacagroup.Ecommerce.Domain.Core
{
    public class CustomersDomain(ICustomersRepository customersRepository) : ICustomersDomain
    {
        private readonly ICustomersRepository customersRepository = customersRepository;

        public bool Delete(string customerId)
        {
            return customersRepository.Delete(customerId);
        }

        public Task<bool> DeleteAsync(string customerId)
        {
            return customersRepository.DeleteAsync(customerId);
        }

        public IEnumerable<Customers> GetAll()
        {
            return customersRepository.GetAll();
        }

        public async Task<IEnumerable<Customers>> GetAllAsync()
        {
            return await customersRepository.GetAllAsync(); 
        }

        public Customers GetById(string customerId)
        {
            return customersRepository.GetById(customerId);
        }

        public async Task<Customers> GetByIdAsync(string customerId)
        {
            return await customersRepository.GetByIdAsync(customerId);
        }

        public bool Insert(Customers customer)
        {
            return customersRepository.Insert(customer);
        }

        public async Task<bool> InsertAsync(Customers customer)
        {
            return await customersRepository.InsertAsync(customer);
        }

        public bool Update(Customers customer)
        {
            return customersRepository.Update(customer);
        }

        public async Task<bool> UpdateAsync(Customers customer)
        {
            return await customersRepository.UpdateAsync(customer);
        }
    }
}
