using PeliculasAPI.Dtos;

namespace PeliculasAPI.Interfaces
{
    public interface IGenderService
    {
        public Task<List<GenderDTO>> GetAllGenders();
        public Task<GenderDTO> GetGenderById(int id);
        public Task<GenderDTO> CreateGender(GenderCreate genderToCreate);
        public Task UpdateGender(int id, GenderCreate genderToUpdate);
        public Task DeleteGender(int id);
    }
}
