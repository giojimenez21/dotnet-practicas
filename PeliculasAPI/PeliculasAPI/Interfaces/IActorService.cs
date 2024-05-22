using PeliculasAPI.Dtos;

namespace PeliculasAPI.Interfaces
{
    public interface IActorService
    {
        public Task<List<ActorDto>> GetAllActors();
        public Task<ActorDto> GetActorById(int id);
        public Task<ActorDto> CreateActor(ActorCreate actorCreate);
        public Task UpdateActorById(int id, ActorCreate actorToUpdate);
        public Task DeleteActorById(int id);   
    }
}
