using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Dtos;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibroDTOAutores>> Get(int id)
        {
            var libro = await context.Libros
                .Include(libro => libro.AutoresLibros)
                .ThenInclude(autorLibro => autorLibro.Autor)
                .Include(libro => libro.Comentarios)
                .FirstOrDefaultAsync(x => x.Id == id);
            return mapper.Map<LibroDTOAutores>(libro);
        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroRequestDTO libroDTO)
        {
            var autores = await context.Autores
                .Where(autor => libroDTO.AutoresIds.Contains(autor.Id))
                .Select(autor => autor.Id)
                .ToListAsync();

            if (libroDTO.AutoresIds.Count != autores.Count)
            {
                return BadRequest("No existe uno de los autores enviados");
            }

            var libro = mapper.Map<Libro>(libroDTO);
            if (libro.AutoresLibros != null)
            {
                for (int i = 0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i;
                }
            }
            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, LibroRequestDTO libroRequestDTO)
        {
            var libroDb = await context.Libros
                .Include(x => x.AutoresLibros)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (libroDb is null)
            {
                return NotFound();
            }

            libroDb = mapper.Map(libroRequestDTO, libroDb);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDTO> libroPathDocu)
        {
            if (libroPathDocu is null)
            {
                return BadRequest();
            }
            var libroDb = await context.Libros.FirstOrDefaultAsync(x => x.Id == id);
            if (libroDb is null)
            {
                return NotFound();
            }
            var libroDTO = mapper.Map<LibroPatchDTO>(libroDb);
            libroPathDocu.ApplyTo(libroDTO, ModelState);
            var esValido = TryValidateModel(libroDTO);
            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(libroDTO, libroDb);
            await context.SaveChangesAsync();
            return NoContent();

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Libros.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Libro { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
