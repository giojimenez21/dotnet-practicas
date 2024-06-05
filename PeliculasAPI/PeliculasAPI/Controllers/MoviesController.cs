using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PeliculasAPI.Dtos;
using PeliculasAPI.Interfaces;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MoviesController(IMovieService movieService) : ControllerBase
    {
        private readonly IMovieService movieService = movieService;

        [HttpGet]  
        public async Task<ActionResult<MoviesIndexDTO>> GetAllMovies()
        {
            var movies = await movieService.GetAllMovies();
            return Ok(movies);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<MovieDTO>>> Filter([FromQuery] FilterMovieDTO filter)
        {
            var movies = await movieService.Filter(filter);
            return Ok(movies);
        }

        [HttpGet("{id}", Name = "GetMovieById")]
        public async Task<ActionResult<MovieDTO>> GetMovieById(int id)
        {
            var movie = await movieService.GetMovieById(id);
            if (movie is null) return NotFound();
            return Ok(movie);
        }

        [HttpPost]
        public async Task<ActionResult> CreateMovie([FromForm] MovieCreateDTO movieCreateDTO)
        {
            var movieCreated = await movieService.CreateMovie(movieCreateDTO);
            return Created(Url.Action("GetMovieById", new { id = movieCreated.Id }), movieCreated);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMovie(int id, [FromForm] MovieCreateDTO movieUpdateDTO)
        {
            await movieService.UpdateMovie(id, movieUpdateDTO);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdatePartialMovie(int id, [FromBody] JsonPatchDocument<MoviePatchDTO> movieDocument)
        {
            await movieService.UpdatePartialMovie(id, movieDocument);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMovie(int id)
        {
            await movieService.DeleteMovie(id);
            return NoContent();
        }
    }
}
