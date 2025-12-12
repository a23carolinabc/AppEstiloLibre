using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace EstiloLibreFront.Validaciones
{
    public class IdObligatorio : ValidationAttribute
    {
        public IdObligatorio() : base() { }
        protected override ValidationResult IsValid(object? value, ValidationContext? validationContext)
        {
            var culturaActual = CultureInfo.CurrentCulture;

            if (value == null || Convert.ToInt32(value) == 0)
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
