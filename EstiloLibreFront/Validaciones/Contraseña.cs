using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace EstiloLibreFront.Validaciones
{
    public class Contraseña : ValidationAttribute
    {
        public Contraseña() : base() { }
        protected override ValidationResult IsValid(object? value, ValidationContext? validationContext)
        {
            CultureInfo culturaActual;
            string propertyName;
            Regex comprobacion;
            string? valor;

            culturaActual = CultureInfo.CurrentCulture;
            propertyName = validationContext.MemberName ?? string.Empty;
            comprobacion = new Regex("^(?=\\w*\\d)(?=\\w*[A-Z])(?=\\w*[a-z])\\S{8,16}$");
            valor = Convert.ToString(value);

            if (string.IsNullOrEmpty(valor) || string.IsNullOrWhiteSpace(valor))
            {
                return ValidationResult.Success;
            }

            if (comprobacion.IsMatch(valor))
            {
                return ValidationResult.Success;                               
            }

            if (culturaActual.Name == "es-ES")
            {
                return new ValidationResult("La contraseña debe tener al menos un dígito, una minúscula, " +
                                                "una mayúscula y entre 8 y 16 caracteres.", new[] { propertyName });
            }
            return new ValidationResult("Ocontrasinal debe ter polo menos un díxito, unha minúscula, " +
                                                "unha maiúscula e entre 8 e 16 caracteres.", new[] { propertyName });
        }
    }
}
