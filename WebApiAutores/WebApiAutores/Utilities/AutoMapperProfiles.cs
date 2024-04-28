using AutoMapper;
using WebApiAutores.Dtos;
using WebApiAutores.Entidades;

namespace WebApiAutores.Utilities
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AutorRequestDTO, Autor>();
            CreateMap<Autor, AutorResponseDTO>();
            CreateMap<LibroRequestDTO, Libro>();
            CreateMap<Libro, LibroResponseDTO>();
        }
    }
}
