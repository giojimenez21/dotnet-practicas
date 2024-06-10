using Suscripciones.Entities;
using WebApiAutores;

namespace Suscripciones.Services
{
    public class LlavesService
    {
        private readonly ApplicationDbContext context;

        public LlavesService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task CreaarLave(string usuarioId, TipoLlave tipoLlave)
        {
            var llave = CrearStringLlave();
            var llaveAPI = new LlaveAPI
            {
                Activa = true,
                Llave = llave,
                TipoLlave = tipoLlave,
                UsuarioId = usuarioId,
            };
            context.Add(llaveAPI);
            await context.SaveChangesAsync();
        }

        public string CrearStringLlave()
        {
            var llave = Guid.NewGuid().ToString().Replace("-", "");
            return llave;
        }
    }
}
