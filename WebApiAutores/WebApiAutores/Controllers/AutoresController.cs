using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Dtos;
using WebApiAutores.Entidades;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class AutoresController : Controller
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
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
        public async Task<ActionResult> Put(int id, [FromBody] AutorRequestDTO autorDTO)
        {
            var autor = mapper.Map<Autor>(autorDTO);
            autor.Id = id;
            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();
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

        [HttpGet("todosAutores")]
        public async Task<ActionResult<List<AutorResponseDTO>>> GetAutores()
        {
            var autores = await context.Autores.ToListAsync();
            return mapper.Map<List<AutorResponseDTO>>(autores);
        }

        [HttpGet("configuraciones")]
        public ActionResult<string> ObtenerConfiguracion()
        {
            return configuration["connectionStrings:defaultConnection"];
        }
    }
}
