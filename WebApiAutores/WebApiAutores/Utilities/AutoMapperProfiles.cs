using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using WebApiAutores.Dtos;
using WebApiAutores.Entidades;

namespace WebApiAutores.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AutorRequestDTO, Autor>();
            CreateMap<Autor, AutorResponseDTO>();
            CreateMap<Autor, AutorResponseDTOLibros>()
                .ForMember(autor => autor.Libros, opciones => opciones.MapFrom(MapLibrosAutores));
            CreateMap<LibroRequestDTO, Libro>()
                .ForMember(libro => libro.AutoresLibros, opciones => opciones.MapFrom(MapAutoresLibros));
            CreateMap<Libro, LibroResponseDTO>();
            CreateMap<Libro, LibroDTOAutores>()
                .ForMember(libro => libro.Autores, opciones => opciones.MapFrom(MapLibroDTOAutores));
            CreateMap<ComentarioRequestDTO, Comentario>();
            CreateMap<Comentario, ComentarioResponseDTO>();
            CreateMap<LibroPatchDTO, Libro>().ReverseMap();
        }

        private List<AutorLibro> MapAutoresLibros(LibroRequestDTO libroRequestDTO, Libro libro)
        {
            var resultado = new List<AutorLibro>();
            if (libroRequestDTO.AutoresIds is null) return resultado;

            foreach (var autorId in libroRequestDTO.AutoresIds)
            {
                resultado.Add(new AutorLibro
                {
                    AutorId = autorId,
                });
            }

            return resultado;
        }

        private List<AutorResponseDTO> MapLibroDTOAutores(Libro libro, LibroResponseDTO libroDTO)
        {
            var resultado = new List<AutorResponseDTO>();
            if(libro.AutoresLibros is null) return resultado;
            foreach (var autorLibro in libro.AutoresLibros)
            {
                resultado.Add(new()
                {
                    Id = autorLibro.AutorId,
                    Nombre = autorLibro.Autor.Nombre
                });
            }
            return resultado;
        }

        private List<LibroResponseDTO> MapLibrosAutores(Autor autor, AutorResponseDTO autorDTO)
        {
            var resultado = new List<LibroResponseDTO>();
            if (autor.AutoresLibros is null) return resultado;
            foreach (var autorLibro in autor.AutoresLibros)
            {
                resultado.Add(new()
                {
                    Id = autorLibro.AutorId,
                    Titulo = autorLibro.Libro.Titulo
                });
            }
            return resultado;
        }
    }
}
