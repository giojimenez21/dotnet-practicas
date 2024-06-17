using Microsoft.EntityFrameworkCore;
using Suscripciones.DTOs;
using Suscripciones.Entities;
using System.Net;
using System.Net.Mime;
using WebApiAutores;

namespace Suscripciones.Middlewares
{
    public static class LimitarPeticionesMiddlewareExtension
    {
        public static IApplicationBuilder UseLimitarPetiuciones(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LimitarPeticionesMiddleware>();
        }
    }

    public class LimitarPeticionesMiddleware
    {
        private readonly RequestDelegate requestDelegate;
        private readonly IConfiguration configuration;

        public LimitarPeticionesMiddleware(RequestDelegate requestDelegate, IConfiguration configuration)
        {
            this.requestDelegate = requestDelegate;
            this.configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext httpContext, ApplicationDbContext context)
        {
            var lmitarPeticionesConfiguration = new LimitarPeticionesConfiguration();
            configuration.GetRequiredSection("limitarPeticiones").Bind(lmitarPeticionesConfiguration);
            var estaLarutaEnListaBlanca = lmitarPeticionesConfiguration
                .ListaBlancaRutas
                .Any(x => httpContext.Request.Path.ToString().Contains(x));
            if (estaLarutaEnListaBlanca)
            {
                await requestDelegate(httpContext);
                return;
            }
            var llaveStringValues = httpContext.Request.Headers["X-API-KEY"];
            if(llaveStringValues.Count == 0)
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsync("Debe proveer la llave en la cabecera");
                return;
            }

            if(llaveStringValues.Count > 1)
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsync("Solo 1 llave debe de estar presente.");
                return;
            }

            var llave = llaveStringValues[0];
            var llaveDB = await context.LlavesAPI.FirstOrDefaultAsync(x => x.Llave == llave);
            if(llaveDB is null)
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsync("La llave no existe");
                return;
            }

            if (!llaveDB.Activa)
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsync("La llave se encuentra inactiva");
                return;
            }

            if(llaveDB.TipoLlave == TipoLlave.Gratuita)
            {
                var cantidadPëticionesHoy = await context.Peticiones
                    .CountAsync(x => x.LlaveId == llaveDB.Id && x.FechaPeticion >= DateTime.Today && x.FechaPeticion < DateTime.Today.AddDays(1));
                if(cantidadPëticionesHoy > lmitarPeticionesConfiguration.PeticionesPorDiaGratuito)
                {
                    httpContext.Response.StatusCode = 429;
                    await httpContext.Response.WriteAsync("Excedio las peticiones por dia.");
                    return;
                }
            }

            var superaRestricciones = PeticionSuperaRestricciones(llaveDB, httpContext);
            if(!superaRestricciones)
            {
                httpContext.Response.StatusCode = 403;
                return;
            }

            var peticion = new Peticion
            {
                LlaveId = llaveDB.Id,
                FechaPeticion = DateTime.UtcNow
            };
            context.Add(peticion);
            await context.SaveChangesAsync();

            await requestDelegate(httpContext);

        }

        private bool PeticionSuperaRestricciones(LlaveAPI llave, HttpContext httpContext)
        {
            var restricciones = llave.RestriccionesDominio.Any() || llave.RestriccionesIP.Any();
            if (!restricciones) return true;
            var peticionSuperaRestricciones = PeticionSuperaRestriccionDominio(llave.RestriccionesDominio, httpContext);
            var peticionSuperaRestriccionesIp = PeticionRestriccionesIp(llave.RestriccionesIP, httpContext);
            return peticionSuperaRestricciones || peticionSuperaRestriccionesIp;
        }

        private bool PeticionSuperaRestriccionDominio(List<RestriccionDominio> restriccionDominios, HttpContext httpContext)
        {
            if(restriccionDominios is null || restriccionDominios.Count == 0) return false;
            var referer = httpContext.Request.Headers["Referer"].ToString();
            if (referer == string.Empty) return false;
            Uri myUri = new(referer);
            string host = myUri.Host;
            var superaRestriccion = restriccionDominios.Any(x => x.Dominio == host);
            return superaRestriccion;
        }

        private bool PeticionRestriccionesIp(List<RestriccionIP> restricciones, HttpContext httpContext)
        {
            if (restricciones is null || restricciones.Count == 0) return false;
            var ip = httpContext.Connection.RemoteIpAddress.ToString();
            if (ip == string.Empty) return false;
            var superaRestriccion = restricciones.Any(x => x.Ip == ip);
            return superaRestriccion;
        }
    }
}
