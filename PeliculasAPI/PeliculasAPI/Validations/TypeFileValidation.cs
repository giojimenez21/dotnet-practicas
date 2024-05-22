using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Validations
{
    public class TypeFileValidation(string[] Types) : ValidationAttribute
    {
        private readonly string[] types = Types;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null) return ValidationResult.Success;
            if (value is not IFormFile formFile) return ValidationResult.Success;
            if(!types.Contains(formFile.ContentType))
            {
                return new ValidationResult("This type is not exist");
            }
            return ValidationResult.Success;
        }
    }
}
