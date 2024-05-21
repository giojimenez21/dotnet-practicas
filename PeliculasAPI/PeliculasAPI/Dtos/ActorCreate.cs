using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Dtos
{
    public class ActorCreate
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public string Picture { get; set; }
    }
}
