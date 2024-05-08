using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Dtos
{
    public class LibroPatchDTO
    {
        [StringLength(100)]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}
