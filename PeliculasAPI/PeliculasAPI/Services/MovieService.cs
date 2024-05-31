using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Context;
using PeliculasAPI.Dtos;
using PeliculasAPI.Entities;
using PeliculasAPI.Interfaces;

namespace PeliculasAPI.Services
{
    public class MovieService(ApplicationDbContext context, IMapper mapper, IFileService fileService) : IMovieService
    {
        private readonly ApplicationDbContext context = context;
        private readonly IMapper mapper = mapper;
        private readonly IFileService fileService = fileService;

        public async Task<MovieDTO> CreateMovie(MovieCreateDTO movieCreate)
        {
            var movie = mapper.Map<Movie>(movieCreate);
            context.Add(movie);
            await context.SaveChangesAsync();
            if (movieCreate.Picture is not null)
            {
                using var memoryStream = new MemoryStream();
                await movieCreate.Picture.CopyToAsync(memoryStream);
                var contentPicture = memoryStream.ToArray();
                var extension = Path.GetExtension(movieCreate.Picture.FileName);
                FileToUpload fileToUpload = new()
                {
                    Container = "movies",
                    Content = memoryStream.ToArray(),
                    Extension = Path.GetExtension(movieCreate.Picture.FileName),
                    ContentType = movieCreate.Picture.ContentType
                };
                movie.Picture = await fileService.SaveFile(fileToUpload);
            }
            var movieDTO = mapper.Map<MovieDTO>(movieCreate);
            return movieDTO;
        }

        public async Task DeleteMovie(int id)
        {
            var movie = await GetMovieById(id) ?? throw new Exception("This movie doesn't exist");
            context.Remove(new Movie { Id = movie.Id });
            await context.SaveChangesAsync();
        }

        public async Task<List<MovieDTO>> GetAllMovies()
        {
            var movies = await context.Movies.ToListAsync();
            return mapper.Map<List<MovieDTO>>(movies);
        }

        public async Task<MovieDTO> GetMovieById(int id)
        {
            var movie = await context.Movies.FirstOrDefaultAsync(m => m.Id == id);
            return mapper.Map<MovieDTO>(movie);
        }

        public async Task UpdateMovie(int id, MovieCreateDTO movieUpdateDTO)
        {
            var movieDTO = await GetMovieById(id) ?? throw new Exception("This actor doesn't exist");
            var movieDB = mapper.Map<Movie>(movieDTO);
            movieDB = mapper.Map(movieUpdateDTO, movieDB);
            if (movieUpdateDTO.Picture is not null)
            {
                using var memoryStream = new MemoryStream();
                await movieUpdateDTO.Picture.CopyToAsync(memoryStream);
                var contentPicture = memoryStream.ToArray();
                var extension = Path.GetExtension(movieUpdateDTO.Picture.FileName);
                FileToUpload fileToUpload = new()
                {
                    Container = "actors",
                    Content = memoryStream.ToArray(),
                    Extension = Path.GetExtension(movieUpdateDTO.Picture.FileName),
                    ContentType = movieUpdateDTO.Picture.ContentType,
                    Route = movieDB.Picture
                };
                movieDTO.Picture = await fileService.EditFile(fileToUpload);
            }
            context.Entry(movieDB).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task UpdatePartialMovie(int id, JsonPatchDocument<MoviePatchDTO> movieDocument)
        {
            var movie = mapper.Map<Movie>(await GetMovieById(id)) ?? throw new Exception("This movie doesn't exist");
            var moviePatch = mapper.Map<MoviePatchDTO>(movie);
            movieDocument.ApplyTo(moviePatch);
            mapper.Map(moviePatch, movie);
            context.Entry(movie).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
