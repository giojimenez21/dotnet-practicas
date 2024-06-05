using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Context;
using PeliculasAPI.Dtos;
using PeliculasAPI.Entities;
using PeliculasAPI.Interfaces;
using PeliculasAPI.Migrations;
using System.Runtime.InteropServices;

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
            AssignOrderActors(movie);
            context.Add(movie);
            await context.SaveChangesAsync();
            AssignOrderActors(movie);
            var movieDTO = mapper.Map<MovieDTO>(movieCreate);
            return movieDTO;
        }

        public async Task DeleteMovie(int id)
        {
            var movie = await GetMovieById(id) ?? throw new Exception("This movie doesn't exist");
            context.Remove(new Movie { Id = movie.Id });
            await context.SaveChangesAsync();
        }

        public async Task<List<MovieDTO>> Filter(FilterMovieDTO filter)
        {
            var moviesQueryable = context.Movies.AsQueryable();
            if(!string.IsNullOrEmpty(filter.Title))
            {
                moviesQueryable = moviesQueryable.Where(movie => movie.Titulo.Contains(filter.Title));
            }

            if(filter.InCinemas)
            {
                moviesQueryable = moviesQueryable.Where(movie => movie.InCinema);
            }

            if(filter.NextReleases)
            {
                moviesQueryable = moviesQueryable.Where(movie => movie.DateRelease > DateTime.Today);
            }

            if(filter.GenderId != 0)
            {
                moviesQueryable = moviesQueryable
                    .Where(movie => movie.MovieGenders.Select(mGender => mGender.GenderId)
                    .Contains(filter.GenderId));
            }

            var movies = await moviesQueryable.ToListAsync();
            return mapper.Map<List<MovieDTO>>(movies);

        }

        public async Task<MoviesIndexDTO> GetAllMovies()
        {
            var nextReleases = await context.Movies
                .Where(m => m.DateRelease > DateTime.Today)
                .OrderBy(m => m.DateRelease)
                .Take(5)
                .ToListAsync();

            var inCinemas = await context.Movies
                .Where(m => m.InCinema)
                .Take(5)
                .ToListAsync();

            var movies = await context.Movies.ToListAsync();
            var result = new MoviesIndexDTO
            {
                InCinemas = mapper.Map<List<MovieDTO>>(inCinemas),
                NextReleases = mapper.Map<List<MovieDTO>>(nextReleases),
                Movies = mapper.Map<List<MovieDTO>>(movies)
            };
            return result;
        }

        public async Task<MovieDTO> GetMovieById(int id)
        {
            var movie = await context.Movies
                .Include(x => x.MoviesActors).ThenInclude(x => x.Actor)
                .Include(x => x.MovieGenders).ThenInclude(x => x.Gender)
                .FirstOrDefaultAsync(m => m.Id == id);
            return mapper.Map<MovieDetailDTO>(movie);
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
            AssignOrderActors(movieDB);
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

        private void AssignOrderActors(Movie movie)
        {
            if(movie.MoviesActors is not null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i;
                }
            }
        }
    }
}
