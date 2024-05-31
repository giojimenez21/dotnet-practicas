using Microsoft.AspNetCore.JsonPatch;
using PeliculasAPI.Dtos;
using PeliculasAPI.Entities;

namespace PeliculasAPI.Interfaces
{
    public interface IMovieService
    {
        public Task<List<MovieDTO>> GetAllMovies();
        public Task<MovieDTO> GetMovieById(int id);
        public Task<MovieDTO> CreateMovie(MovieCreateDTO movieCreate);
        public Task UpdateMovie(int id, MovieCreateDTO movieUpdateDTO);
        public Task UpdatePartialMovie(int id, JsonPatchDocument<MoviePatchDTO> movieDocument);
        public Task DeleteMovie(int id);

    }
}
