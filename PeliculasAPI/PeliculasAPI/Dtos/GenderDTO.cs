using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Dtos
{
    public class GenderDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
