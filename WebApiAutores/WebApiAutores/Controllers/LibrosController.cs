using AutoMapper;
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

            if(libroDTO.AutoresIds.Count != autores.Count)
            {
                return BadRequest("No existe uno de los autores enviados");
            }

            var libro = mapper.Map<Libro>(libroDTO);
            if(libro.AutoresLibros != null)
            {
                for(int i = 0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i;
                }
            }
            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
