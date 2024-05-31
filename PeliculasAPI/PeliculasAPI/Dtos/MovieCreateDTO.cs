using Microsoft.AspNetCore.Mvc;
using PeliculasAPI.Helpers;
using PeliculasAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Dtos
{
    public class MovieCreateDTO: MoviePatchDTO
    {
        public IFormFile Picture { get; set; }

        public List<int> GendersIds { get; set; }

        public List<ActorMovieCreateDTO> Actors { get; set; }
    }
}
