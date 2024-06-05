namespace PeliculasAPI.Dtos
{
    public class FilterMovieDTO
    {
        public string Title { get; set; }
        public int GenderId { get; set; }
        public bool InCinemas { get; set; }
        public bool NextReleases { get; set; }
    }
}
