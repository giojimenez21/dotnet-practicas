using PeliculasAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Dtos
{
    public class ActorCreate
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }

        [SizeFileValidation(MaxSize: 4)]
        [TypeFileValidation(Types: ["image/jpeg", "image/png"])]
        public IFormFile Picture { get; set; }
    }
}
