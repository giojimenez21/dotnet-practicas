using System.ComponentModel.DataAnnotations;
using WebApiAutores.Entidades;

namespace WebApiAutores.Dtos
{
    public class AutorResponseDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}
