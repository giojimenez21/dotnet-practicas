using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Context;
using PeliculasAPI.Dtos;
using PeliculasAPI.Interfaces;

namespace PeliculasAPI.Services
{
    public class ActorService(ApplicationDbContext context, IMapper mapper) : IActorService
    {
        public ApplicationDbContext context = context;
        public IMapper mapper = mapper;

        public async Task<List<ActorDto>> GetAllActors()
        {
            var allActors = await context.Actors.ToListAsync();
            return mapper.Map<List<ActorDto>>(allActors);
        }

        public async Task<ActorDto> GetActorById(int id)
        {
            var actor = await context.Actors.FirstOrDefaultAsync(a => a.Id == id);
            return mapper.Map<ActorDto>(actor);
        }


    }
}
