using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Dtos
{
    public class AdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
