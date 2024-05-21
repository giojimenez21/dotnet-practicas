using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Context;
using PeliculasAPI.Dtos;
using PeliculasAPI.Entities;
using PeliculasAPI.Interfaces;

namespace PeliculasAPI.Services
{
    public class GenderService(ApplicationDbContext context, IMapper mapper) : IGenderService
    {
        private readonly ApplicationDbContext context = context;
        private readonly IMapper mapper = mapper;

        public async Task<List<GenderDTO>> GetAllGenders()
        {
            var genders = await context.Genders.ToListAsync();
            return mapper.Map<List<GenderDTO>>(genders);
        }

        public async Task<GenderDTO> GetGenderById(int id)
        {
            var gender = await context.Genders.FirstOrDefaultAsync(g => g.Id == id);
            return mapper.Map<GenderDTO>(gender);
        }
        public async Task<GenderDTO> CreateGender(GenderCreate genderToCreate)
        {
            var gender = mapper.Map<Gender>(genderToCreate);
            context.Add(gender);
            await context.SaveChangesAsync();
            return mapper.Map<GenderDTO>(gender);
        }

        public async Task UpdateGender(int id, GenderCreate genderToUpdate)
        {
            var gender = mapper.Map<Gender>(genderToUpdate);
            gender.Id = id;
            context.Entry(gender).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteGender(int id)
        {
            var gender = await GetGenderById(id) ?? throw new Exception("This gender doesn't exist");
            context.Remove(new Gender { Id = gender.Id });
            await context.SaveChangesAsync();
        }
    }
}
