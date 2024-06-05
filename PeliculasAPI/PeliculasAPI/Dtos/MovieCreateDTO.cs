using Microsoft.AspNetCore.Mvc;
using PeliculasAPI.Helpers;
using PeliculasAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Dtos
{
    public class MovieCreateDTO: MoviePatchDTO
    {
        public IFormFile Picture { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GendersIds { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorMovieCreateDTO>>))]
        public List<ActorMovieCreateDTO> Actors { get; set; }
    }
}
