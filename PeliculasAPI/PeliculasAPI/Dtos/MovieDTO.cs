using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Dtos
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public bool InCinema { get; set; }
        public DateTime DateRelease { get; set; }
        public string Picture { get; set; }
    }
}
