using Pacagroup.Ecommerce.Application.DTO;
using Pacagroup.Ecommerce.Transversal.Common;

namespace Pacagroup.Ecommerce.Application.Interface
{
    public interface ICustomerApplication
    {
        public Response<bool> Insert(CustomersDTO customer);
        public Response<bool> Update(CustomersDTO customer);
        public Response<bool> Delete(string customerId);
        public Response<CustomersDTO> GetById(string customerId);
        public Response<IEnumerable<CustomersDTO>> GetAll();
        public Task<Response<bool>> InsertAsync(CustomersDTO customer);
        public Task<Response<bool>> UpdateAsync(CustomersDTO customer);
        public Task<Response<bool>> DeleteAsync(string customerId);
        public Task<Response<CustomersDTO>> GetByIdAsync(string customerId);
        public Task<Response<IEnumerable<CustomersDTO>>> GetAllAsync();
    }
}
