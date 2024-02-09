using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos;

namespace Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios
{
    public static class ValidacionUtilitario
    {

        public static OperacionDto<T> ValidarModelo<T>(Object objeto)
        {
            var contexto = new ValidationContext(objeto, null, null);
            var errores = new List<string>();
            var resultados = new List<ValidationResult>();
            var esValido = Validator.TryValidateObject(objeto, contexto, resultados, true);

            if (!esValido)
            {
                return new OperacionDto<T>(CodigosOperacionDto.Invalido, resultados.Select(e => e.ErrorMessage ?? "").ToList());
            }

            return new OperacionDto<T>(Activator.CreateInstance<T>());
        }

        public static bool ValidaNumeroTelefono(string cadena)
        {
            var respuesta = false;
            if (cadena.Length != 9)
            {
                return false;
            }
            try
            {
                var x = int.Parse(cadena);
                respuesta = true;
            }
            catch
            {
                respuesta = false;
            }
            return respuesta;
        }


        public static bool ValidarEmail(string email)
        {
            String expresion; expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }



    }

    public class HasNestedValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var isValid = true;
            var result = ValidationResult.Success;

            var nestedValidationProperties = value.GetType().GetProperties()
                .Where(p => IsDefined(p, typeof(ValidationAttribute)))
                .OrderBy(p => p.Name);//Not the best order, but at least known and repeatable.

            foreach (var property in nestedValidationProperties)
            {
                var validators = GetCustomAttributes(property, typeof(ValidationAttribute)) as ValidationAttribute[];

                if (validators == null || validators.Length == 0) continue;

                foreach (var validator in validators)
                {
                    var propertyValue = property.GetValue(value, null);

                    result = validator.GetValidationResult(propertyValue, new ValidationContext(value, null, null));
                    if (result == ValidationResult.Success) continue;

                    isValid = false;
                    break;
                }

                if (!isValid)
                {
                    break;
                }
            }
            return result;

        }


    }

}
