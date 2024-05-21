using Microsoft.AspNetCore.Mvc;
using PeliculasAPI.Dtos;
using PeliculasAPI.Entities;
using PeliculasAPI.Interfaces;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/genders")]
    public class GenderController(IGenderService genderService) : ControllerBase
    {
        private readonly IGenderService genderService = genderService;

        [HttpGet]
        public async Task<ActionResult<List<GenderDTO>>> GetGenders()
        {
            var genders = await genderService.GetAllGenders();
            return Ok(genders);
        }

        [HttpGet("{id:int}", Name = "ObtenerGeneroId")]
        public async Task<ActionResult<GenderDTO>> GetGenderById(int id)
        {
            var gender = await genderService.GetGenderById(id);
            if (gender == null) return NotFound(new { message = "That gender doesn't exist" });
            return Ok(gender);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GenderCreate genderCreate)
        {
            var genderCreated = await genderService.CreateGender(genderCreate);
            return Created(Url.Action("ObtenerGeneroId", new { id = genderCreated.Id }), genderCreated);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateGender(int id, [FromBody] GenderCreate genderCreate)
        {
            await genderService.UpdateGender(id, genderCreate);
            return NoContent();
        }

        [HttpDelete("{id}")]
        
        public async Task<ActionResult> DeleteGender(int id) 
        {
            await genderService.DeleteGender(id);
            return NoContent();
        }

    }
}
