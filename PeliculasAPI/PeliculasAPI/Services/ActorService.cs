using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Context;
using PeliculasAPI.Dtos;
using PeliculasAPI.Entities;
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

        public async Task<ActorDto> CreateActor(ActorCreate actorCreate)
        {
            var actor = mapper.Map<Actor>(actorCreate);
            context.Add(actor);
            await context.SaveChangesAsync();
            return mapper.Map<ActorDto>(actor);
        }

        public async Task UpdateActorById(int id, ActorCreate actorToUpdate)
        {
            var actor = mapper.Map<Actor>(actorToUpdate);
            actor.Id = id;
            context.Entry(actor).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteActorById(int id)
        {
            _ = await GetActorById(id) ?? throw new Exception("This actor doesn' exist");
            context.Remove(new Actor { Id = id });
            await context.SaveChangesAsync();
        }
    }
}
