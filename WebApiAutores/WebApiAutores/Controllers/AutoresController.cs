using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Dtos;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //[HttpGet]
        //public async Task<ActionResult<List<Autor>>> Get()
        //{
        //    return await context.Autores.Include(x => x.Libros).ToListAsync();
        //}

        [HttpPost]
        public async Task<ActionResult> Post(AutorRequestDTO autorDto)
        {
            var autor = mapper.Map<Autor>(autorDto);
            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] Autor autor)
        {
            if (autor.Id != id)
            {
                return BadRequest("El id del autor no coincide con el id del url");
            }

            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Autor { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<AutorResponseDTO>>> GetByName([FromRoute] string nombre)
        {
            var autores = await context.Autores.Where(autor => autor.Nombre.Contains(nombre)).ToListAsync();
            return mapper.Map<List<AutorResponseDTO>>(autores);
        }
    }
}
