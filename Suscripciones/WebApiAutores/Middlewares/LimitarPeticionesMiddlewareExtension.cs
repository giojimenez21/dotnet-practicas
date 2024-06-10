using Suscripciones.DTOs;
using System.Net;
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
            var llaveStringValues = httpContext.Request.Headers["X-API-KEY"];
            if(llaveStringValues.Count == 0)
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsync("Debe proveer la llave en la cabecera");
                return;
            }

            await requestDelegate(httpContext);

        }
    }
}
