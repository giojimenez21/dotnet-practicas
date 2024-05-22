using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Validations
{
    public class SizeFileValidation(int MaxSize) : ValidationAttribute
    {
        private readonly int maxSize = MaxSize;
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null) return ValidationResult.Success;
            if (value is not IFormFile formFile) return ValidationResult.Success;

            if (formFile.Length > maxSize * 1024 * 1024) return new ValidationResult($"The size of file is bigger than {maxSize}");

            return ValidationResult.Success;
        }
    }
}
