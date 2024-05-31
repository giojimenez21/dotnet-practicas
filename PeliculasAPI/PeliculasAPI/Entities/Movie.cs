using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Titulo { get; set; }
        public bool InCinema { get; set; }
        public DateTime DateRelease { get; set; }
        public string Picture { get; set; }
        public List<MoviesActors> MoviesActors { get; set; }
        public List<MoviesGenders> MovieGenders { get; set; }
    }
}
