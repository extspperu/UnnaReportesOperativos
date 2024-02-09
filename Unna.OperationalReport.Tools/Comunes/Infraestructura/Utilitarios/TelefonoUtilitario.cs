using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios
{
    public static class TelefonoUtilitario
    {

        public static bool EsTelefonoValido(string cadena)
        {
            if (string.IsNullOrWhiteSpace(cadena))
            {
                return false;
            }

            var numero = cadena.Trim();
            if (numero.Length != 9)
            {
                return false;
            }

            return int.TryParse(numero, out _);
        }

    }
}
