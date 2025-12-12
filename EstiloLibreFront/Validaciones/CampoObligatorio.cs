using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace EstiloLibreFront.Validaciones
{
    public class CampoObligatorio : ValidationAttribute
    {
        public CampoObligatorio() : base() { }
        protected override ValidationResult IsValid(object? value, ValidationContext? validationContext)
        {
            var culturaActual = CultureInfo.CurrentCulture;

            if (value == null || string.IsNullOrEmpty(value.ToString()) || string.IsNullOrWhiteSpace(value.ToString()))
            {
                var propertyName = validationContext.MemberName ?? string.Empty;

                if (culturaActual.Name == "es-ES")
                {
                        return new ValidationResult("Obligatorio", new[] { propertyName });
                }
                return new ValidationResult("Obrigatorio", new[] { propertyName });                
            }
            return ValidationResult.Success;
        }
    }
}
