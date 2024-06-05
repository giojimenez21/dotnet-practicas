namespace PeliculasAPI.Dtos
{
    public class MoviesIndexDTO
    {
        public List<MovieDTO> NextReleases { get; set; }
        public List<MovieDTO> InCinemas { get; set; }
        public List<MovieDTO> Movies { get; set; }
    }
}
