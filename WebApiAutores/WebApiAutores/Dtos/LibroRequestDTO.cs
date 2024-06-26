﻿using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Dtos
{
    public class LibroRequestDTO
    {
        [StringLength(100)]
        public string Titulo { get; set; }
        public List<int> AutoresIds { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}
