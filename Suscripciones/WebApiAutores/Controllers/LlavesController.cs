using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Suscripciones.DTOs;
using Suscripciones.Services;
using WebApiAutores;

namespace Suscripciones.Controllers
{
    [ApiController]
    [Route("api/llaves")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LlavesController : CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly LlavesService llavesService;

        public LlavesController(ApplicationDbContext context, IMapper mapper, LlavesService llavesService)
        {
            this.context = context;
            this.mapper = mapper;
            this.llavesService = llavesService;
        }

        [HttpGet]
        public async Task<List<LlaveDTO>> MisLlaves()
        {
            var usuarioId = ObtenerUsuarioId();
            var llaves = await context.LlavesAPI.Where(x => x.UsuarioId == usuarioId).ToListAsync();
            return mapper.Map<List<LlaveDTO>>(llaves);
        }

        [HttpPost]
        public async Task<ActionResult> CrearLlave(CrearLlaveDTO crearLlaveDTO)
        {
            var usuarioId = ObtenerUsuarioId();
            if(crearLlaveDTO.TipoLlave == Entities.TipoLlave.Gratuita)
            {
                var usuarioTieneGratuita = await context.LlavesAPI
                    .AnyAsync(x => x.UsuarioId == usuarioId && x.TipoLlave == Entities.TipoLlave.Gratuita);

                if(usuarioTieneGratuita) return BadRequest("El usuario ya tiene una llave gratuita");
            }

            await llavesService.CreaarLave(usuarioId, crearLlaveDTO.TipoLlave);
            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> ActualizarLlave(ActualizarLlaveDTO actualizarLlaveDTO)
        {
            var usuarioId = ObtenerUsuarioId();
            var llaveDB = await context.LlavesAPI.FirstOrDefaultAsync(x => x.Id == actualizarLlaveDTO.LlaveId);
            if (llaveDB == null) return NotFound();

            if (usuarioId != llaveDB.UsuarioId) return Forbid();

            if(actualizarLlaveDTO.ActualizarLlave)
            {
                llaveDB.Llave = llavesService.CrearStringLlave();
            }

            llaveDB.Activa = actualizarLlaveDTO.Activa;
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
