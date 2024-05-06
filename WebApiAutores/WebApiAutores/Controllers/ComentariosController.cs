using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Dtos;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")]
    public class ComentariosController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ComentariosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> Post(int libroId, ComentarioRequestDTO comentarioDTO)
        {
            var existeLibro = await context.Libros.AnyAsync(libro => libro.Id == libroId);
            if (!existeLibro)
            {
                return NotFound();
            }

            var comentario = mapper.Map<Comentario>(comentarioDTO);
            comentario.LibroId = libroId;
            context.Add(comentario);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<ComentarioResponseDTO>>> Get(int libroId)
        {
            var comentarios = await context.Comentarios.Where(comentario => comentario.LibroId == libroId).ToListAsync();
            return Ok(mapper.Map<List<ComentarioResponseDTO>>(comentarios));
        }
    }
}
