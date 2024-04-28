using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Dtos
{
    public class AutorRequestDTO
    {
        [Required]
        [StringLength(250)]
        public string Nombre { get; set; }
    }
}
