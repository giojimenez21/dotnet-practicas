using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Dtos
{
    public class MoviePatchDTO
    {
        [Required]
        [StringLength(300)]
        public string Titulo { get; set; }
        public bool InCinema { get; set; }
        public DateTime DateRelease { get; set; }
    }
}
