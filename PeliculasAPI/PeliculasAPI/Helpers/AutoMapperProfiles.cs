using AutoMapper;
using PeliculasAPI.Dtos;
using PeliculasAPI.Entities;

namespace PeliculasAPI.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Gender, GenderDTO>().ReverseMap();
            CreateMap<GenderCreate, Gender>();
            CreateMap<Actor, ActorDto>().ReverseMap();
            CreateMap<ActorCreate, Actor>().ForMember(a => a.Picture, options => options.Ignore());
            CreateMap<ActorPatchDTO, Actor>().ReverseMap();

            CreateMap<Movie, MovieDTO>().ReverseMap();
            CreateMap<MovieCreateDTO, Movie>()
                .ForMember(m => m.Picture, options => options.Ignore())
                .ForMember(x => x.MovieGenders, options => options.MapFrom(MapMoviesGenders))
                .ForMember(x => x.MoviesActors, options => options.MapFrom(MapMoviesActors));
            CreateMap<MoviePatchDTO, Movie>().ReverseMap();
        }

        private List<MoviesGenders> MapMoviesGenders(MovieCreateDTO movieCreateDTO, Movie movie)
        {
            var result = new List<MoviesGenders>();
            if (movieCreateDTO.GendersIds is null) return result;
            movieCreateDTO.GendersIds.ForEach(id =>
            {
                result.Add(new MoviesGenders
                {
                    GenderId = id
                });
            });
            return result;
        }
        private List<MoviesActors> MapMoviesActors(MovieCreateDTO movieCreateDTO, Movie movie)
        {
            var result = new List<MoviesActors>();
            if (movieCreateDTO.Actors is null) return result;
            movieCreateDTO.Actors.ForEach(actor =>
            {
                result.Add(new MoviesActors
                {
                    ActorId = actor.ActorId,
                    Character = actor.NameCharacter
                });
            });
            return result;
        }
    }
}
