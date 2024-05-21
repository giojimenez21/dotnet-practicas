using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Entities
{
    public class Gender
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
