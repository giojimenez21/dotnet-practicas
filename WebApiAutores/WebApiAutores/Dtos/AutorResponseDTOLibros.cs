using System.ComponentModel.DataAnnotations;
using WebApiAutores.Entidades;

namespace WebApiAutores.Dtos
{
    public class AutorResponseDTOLibros: AutorResponseDTO
    {
        public List<LibroResponseDTO> Libros { get; set; }
    }
}
