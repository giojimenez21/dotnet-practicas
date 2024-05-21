using AutoMapper;
using PeliculasAPI.Dtos;
using PeliculasAPI.Entities;

namespace PeliculasAPI.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Gender, GenderDTO>().ReverseMap();
            CreateMap<GenderCreate, Gender>();
            CreateMap<Actor, ActorDto>().ReverseMap();
        }
    }
}
