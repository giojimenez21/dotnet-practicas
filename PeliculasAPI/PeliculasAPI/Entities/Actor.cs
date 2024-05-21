using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public string Picture { get; set; }
    }
}
