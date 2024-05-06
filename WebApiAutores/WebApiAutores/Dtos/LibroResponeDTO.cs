using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Dtos
{
    public class LibroResponseDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public List<ComentarioResponseDTO> Comentarios { get; set; }
    }
}
