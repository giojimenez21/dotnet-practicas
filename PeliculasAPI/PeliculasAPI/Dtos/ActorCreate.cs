using PeliculasAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Dtos
{
    public class ActorCreate: ActorPatchDTO
    {
        [SizeFileValidation(MaxSize: 4)]
        [TypeFileValidation(Types: ["image/jpeg", "image/png"])]
        public IFormFile Picture { get; set; }
    }
}
