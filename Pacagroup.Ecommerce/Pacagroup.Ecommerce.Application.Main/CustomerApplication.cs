using AutoMapper;
using Pacagroup.Ecommer.Domain.Interface;
using Pacagroup.Ecommerce.Application.DTO;
using Pacagroup.Ecommerce.Application.Interface;
using Pacagroup.Ecommerce.Domain.Entity;
using Pacagroup.Ecommerce.Transversal.Common;

namespace Pacagroup.Ecommerce.Application.Main
{
    public class CustomerApplication(ICustomersDomain customersDomain, IMapper mapper) : ICustomerApplication
    {
        private readonly IMapper mapper = mapper;
        private readonly ICustomersDomain customersDomain = customersDomain;

        public Response<bool> Delete(string customerId)
        {
            var response = new Response<bool>();
            try
            {
                response.Data = customersDomain.Delete(customerId);
                if (response.Data)
                {
                    response.IsSucces = true;
                    response.Message = "Eliminacion exitosa";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public Task<Response<bool>> DeleteAsync(string customerId)
        {
            throw new NotImplementedException();
        }

        public Response<IEnumerable<CustomersDTO>> GetAll()
        {
            var response = new Response<IEnumerable<CustomersDTO>>();
            try
            {
                var customerDomain = customersDomain.GetAll();
                response.Data = mapper.Map<IEnumerable<CustomersDTO>>(customerDomain);
                if (response.Data is not null)
                {
                    response.IsSucces = true;
                    response.Message = "Consulta exitosa";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response<IEnumerable<CustomersDTO>>> GetAllAsync()
        {
            var response = new Response<IEnumerable<CustomersDTO>>();
            try
            {
                var customerDomain = await customersDomain.GetAllAsync();
                response.Data = mapper.Map<IEnumerable<CustomersDTO>>(customerDomain);
                if (response.Data is not null)
                {
                    response.IsSucces = true;
                    response.Message = "Consulta exitosa";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public Response<CustomersDTO> GetById(string customerId)
        {
            var response = new Response<CustomersDTO>();
            try
            {
                var customerDomain = customersDomain.GetById(customerId);
                response.Data = mapper.Map<CustomersDTO>(customerDomain);
                if (response.Data is not null)
                {
                    response.IsSucces = true;
                    response.Message = "Consulta exitosa";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response<CustomersDTO>> GetByIdAsync(string customerId)
        {
            var response = new Response<CustomersDTO>();
            try
            {
                var customerDomain = await customersDomain.GetByIdAsync(customerId);
                response.Data = mapper.Map<CustomersDTO>(customerDomain);
                if (response.Data is not null)
                {
                    response.IsSucces = true;
                    response.Message = "Consulta exitosa";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public Response<bool> Insert(CustomersDTO customer)
        {
            var response = new Response<bool>();
            try
            {
                var customerMap = mapper.Map<Customers>(customer);
                response.Data = customersDomain.Insert(customerMap);
                if (response.Data)
                {
                    response.IsSucces = true;
                    response.Message = "Registro exitoso";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message; 
            }
            return response;
        }

        public async Task<Response<bool>> InsertAsync(CustomersDTO customer)
        {
            var response = new Response<bool>();
            try
            {
                var customerMap = mapper.Map<Customers>(customer);
                response.Data = await customersDomain.InsertAsync(customerMap);
                if (response.Data)
                {
                    response.IsSucces = true;
                    response.Message = "Registro exitoso";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public Response<bool> Update(CustomersDTO customer)
        {
            var response = new Response<bool>();
            try
            {
                var customerMap = mapper.Map<Customers>(customer);
                response.Data = customersDomain.Update(customerMap);
                if (response.Data)
                {
                    response.IsSucces = true;
                    response.Message = "Actualizacion exitosa";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response<bool>> UpdateAsync(CustomersDTO customer)
        {
            var response = new Response<bool>();
            try
            {
                var customerMap = mapper.Map<Customers>(customer);
                response.Data = await customersDomain.UpdateAsync(customerMap);
                if (response.Data)
                {
                    response.IsSucces = true;
                    response.Message = "Actualizacion exitosa";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
