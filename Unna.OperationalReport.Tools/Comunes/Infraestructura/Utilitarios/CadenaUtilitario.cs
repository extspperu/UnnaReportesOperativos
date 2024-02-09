using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios
{
    public static class CadenaUtilitario
    {
        public static bool ComienzaConNumero(string cadena)
        {

            if (string.IsNullOrWhiteSpace(cadena) || cadena.Length < 1)
            {
                return false;
            }

            var letra = cadena.ElementAt(0).ToString();

            if (letra.All(c => (c >= 48 && c <= 57)))
            {
                return true;
            }

            return false;
        }

        public static string GenerarCadenaNumericaRandom(int longitud) {
            var random = new Random();
            var chars = "0123456789";
            return new string(Enumerable.Repeat(chars, longitud)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
