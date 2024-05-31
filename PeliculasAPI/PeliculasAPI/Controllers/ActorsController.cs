using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PeliculasAPI.Dtos;
using PeliculasAPI.Interfaces;
using PeliculasAPI.Services;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/actors")]
    public class ActorsController(IActorService actorService, IFileService fileService) : ControllerBase
    {
        private readonly IActorService actorService = actorService;
        public readonly IFileService fileService = fileService;

        [HttpGet]
        public async Task<ActionResult<List<ActorDto>>> GetAllActors()
        {
            var actors = await actorService.GetAllActors();
            return Ok(actors);
        }

        [HttpGet("{id}", Name = "GetActorById")]
        public async Task<ActionResult<ActorDto>> GetActorById(int id)
        {
            var actor = await actorService.GetActorById(id);
            if (actor is null) return NotFound(new { message = "This actor doesn't exist" });
            return Ok(actor);
        }

        [HttpPost]
        public async Task<ActionResult<ActorDto>> CreateActor([FromForm] ActorCreate actorCreate)
        {
            var actorCreated = await actorService.CreateActor(actorCreate);
            return Created(Url.Action("GetActorById", new { id = actorCreated.Id }), actorCreated);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateActor(int id, [FromForm] ActorCreate actorToUpdate)
        {
            await actorService.UpdateActorById(id, actorToUpdate);
            return NoContent();
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteActor(int id)
        {
            await actorService.DeleteActorById(id);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdatePartialActor(int id, [FromBody] JsonPatchDocument<ActorPatchDTO> actorDocument)
        {
            await actorService.UpdatePartialActor(id, actorDocument);
            return NoContent();
        }
    }
}
