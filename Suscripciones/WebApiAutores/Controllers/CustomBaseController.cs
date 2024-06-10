using Microsoft.AspNetCore.Mvc;

namespace Suscripciones.Controllers
{
    public class CustomBaseController : ControllerBase
    {
       protected string ObtenerUsuarioId()
        {
            return HttpContext.User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
        }
    }
}
