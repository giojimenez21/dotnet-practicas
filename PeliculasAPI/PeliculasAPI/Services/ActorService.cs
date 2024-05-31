using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Context;
using PeliculasAPI.Dtos;
using PeliculasAPI.Entities;
using PeliculasAPI.Interfaces;

namespace PeliculasAPI.Services
{
    public class ActorService(ApplicationDbContext context, IMapper mapper, IFileService fileService) : IActorService
    {
        public ApplicationDbContext context = context;
        public IMapper mapper = mapper;
        private readonly IFileService fileService = fileService;

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
            if (actorCreate.Picture is not null)
            {
                using var memoryStream = new MemoryStream();
                await actorCreate.Picture.CopyToAsync(memoryStream);
                var contentPicture = memoryStream.ToArray();
                var extension = Path.GetExtension(actorCreate.Picture.FileName);
                FileToUpload fileToUpload = new()
                {
                    Container = "actors",
                    Content = memoryStream.ToArray(),
                    Extension = Path.GetExtension(actorCreate.Picture.FileName),
                    ContentType = actorCreate.Picture.ContentType
                };
                actor.Picture = await fileService.SaveFile(fileToUpload);
            }
            context.Add(actor);
            await context.SaveChangesAsync();
            return mapper.Map<ActorDto>(actor);
        }

        public async Task UpdateActorById(int id, ActorCreate actorToUpdate)
        {
            var actorDto = await GetActorById(id) ?? throw new Exception("This actor doesn't exist");
            var actorDB = mapper.Map<Actor>(actorDto);
            actorDB = mapper.Map(actorToUpdate, actorDB);
            if (actorToUpdate.Picture is not null)
            {
                using var memoryStream = new MemoryStream();
                await actorToUpdate.Picture.CopyToAsync(memoryStream);
                var contentPicture = memoryStream.ToArray();
                var extension = Path.GetExtension(actorToUpdate.Picture.FileName);
                FileToUpload fileToUpload = new()
                {
                    Container = "actors",
                    Content = memoryStream.ToArray(),
                    Extension = Path.GetExtension(actorToUpdate.Picture.FileName),
                    ContentType= actorToUpdate.Picture.ContentType,
                    Route = actorDB.Picture
                };
                actorDto.Picture = await fileService.EditFile(fileToUpload);
            }
            context.Entry(actorDB).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteActorById(int id)
        {
            _ = await GetActorById(id) ?? throw new Exception("This actor doesn't exist");
            context.Remove(new Actor { Id = id });
            await context.SaveChangesAsync();
        }

        public async Task UpdatePartialActor(int id, JsonPatchDocument<ActorPatchDTO> actorDocument)
        {
            var actor = mapper.Map<Actor>(await GetActorById(id)) ?? throw new Exception("This actor doesn't exist");
            var actorPatch = mapper.Map<ActorPatchDTO>(actor);
            actorDocument.ApplyTo(actorPatch);
            mapper.Map(actorPatch, actor);
            context.Entry(actor).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
