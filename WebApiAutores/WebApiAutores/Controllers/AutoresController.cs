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
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("{id:int}", Name = "obtenerAutor")]
        public async Task<ActionResult<AutorResponseDTOLibros>> Get(int id)
        {
            var autor = await context.Autores
                .Include(autor => autor.AutoresLibros)
                .ThenInclude(autorLibro => autorLibro.Libro)
                .FirstOrDefaultAsync(autor => autor.Id == id);

            if (autor is null) return NoContent();
            return Ok(mapper.Map<AutorResponseDTOLibros>(autor));
        }

        [HttpPost]
        public async Task<ActionResult> Post(AutorRequestDTO autorRequestDto)
        {
            var autor = mapper.Map<Autor>(autorRequestDto);
            context.Add(autor);
            await context.SaveChangesAsync();
            var autorDto = mapper.Map<AutorResponseDTO>(autor);
            return CreatedAtRoute("obtenerAutor", new { id = autor.Id }, autorDto);
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

        [HttpGet("ejemplo")]
        public string Ejemplo()
        {
            httpContextAccessor.HttpContext.Session.SetString("nombre", "Gio");
            var nombre = httpContextAccessor.HttpContext.Session.GetString("nombre");
            return nombre;
        }

        [HttpGet("ejemplo2")]
        public string Ejemplo2()
        {
            var nombre = httpContextAccessor.HttpContext.Session.GetString("nombre");
            return nombre;
        }
    }
}
