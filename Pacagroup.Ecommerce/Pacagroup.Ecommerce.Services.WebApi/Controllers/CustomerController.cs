using Microsoft.AspNetCore.Mvc;
using Pacagroup.Ecommerce.Application.DTO;
using Pacagroup.Ecommerce.Application.Interface;

namespace Pacagroup.Ecommerce.Services.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerController(ICustomerApplication customerApplication) : ControllerBase
    {
        private readonly ICustomerApplication customerApplication = customerApplication;

        [HttpPost]
        public IActionResult Insert([FromBody] CustomersDTO customersDTO)
        {
            if (customersDTO is null) return BadRequest();
            var response = customerApplication.Insert(customersDTO);
            if(response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }

        [HttpPut]
        public IActionResult Update([FromBody] CustomersDTO customersDTO)
        {
            if (customersDTO is null) return BadRequest();
            var response = customerApplication.Update(customersDTO);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }

        [HttpDelete("{customerId}")]
        public IActionResult Delete(string customerId)
        {
            var response = customerApplication.Delete(customerId);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }

        [HttpGet("{customerId}")]
        public IActionResult GetById(string customerId)
        {
            var response = customerApplication.GetById(customerId);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = customerApplication.GetAll();
            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }

        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody] CustomersDTO customersDTO)
        {
            if (customersDTO is null) return BadRequest();
            var response = await customerApplication.InsertAsync(customersDTO);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] CustomersDTO customersDTO)
        {
            if (customersDTO is null) return BadRequest();
            var response = await customerApplication.UpdateAsync(customersDTO);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }

        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteAsync(string customerId)
        {
            var response = await customerApplication.DeleteAsync(customerId);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetByIdAsync(string customerId)
        {
            var response = await customerApplication.GetByIdAsync(customerId);
            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await customerApplication.GetAllAsync();
            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }

    }
}
