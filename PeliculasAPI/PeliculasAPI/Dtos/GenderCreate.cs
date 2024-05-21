using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Dtos
{
    public class GenderCreate
    {
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
    }
}
