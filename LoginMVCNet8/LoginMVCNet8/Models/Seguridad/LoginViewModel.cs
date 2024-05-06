using System.ComponentModel.DataAnnotations;

namespace LoginMVCNet8.Models.Seguridad
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El email es requerido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El password es requerido")]
        public string Password { get; set; }
    }
}
