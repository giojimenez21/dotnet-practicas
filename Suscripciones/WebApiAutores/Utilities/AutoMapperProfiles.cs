using AutoMapper;
using Suscripciones.DTOs;
using Suscripciones.Entities;

namespace Suscripciones.Utilities
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<LlaveAPI, LlaveDTO>();
        }
    }
}
