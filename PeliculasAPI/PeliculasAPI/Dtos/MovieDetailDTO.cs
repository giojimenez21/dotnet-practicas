namespace PeliculasAPI.Dtos
{
    public class MovieDetailDTO: MovieDTO
    {
        public List<GenderDTO> Gender { get; set; }
        public List<ActorDetailDTO> Actors { get; set; }
    }
}
