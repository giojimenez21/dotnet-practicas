using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Dtos
{
    public class ActorPatchDTO
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
