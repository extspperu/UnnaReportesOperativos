using System;
using System.Collections.Generic;
using System.Text;

namespace Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios
{
    public static class FormatoUtilitario
    {
        public static string ConvertirABase64String(string texto)
        {
            byte[] concatenated = Encoding.ASCII.GetBytes(texto);
            return Convert.ToBase64String(concatenated);
        }

        public static string ConvertirAscii(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return texto;
            }

            return Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(texto));
        }

        public static string ReemplazarSaltoLinea(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return texto;
            }

            string replaceWith = "";
            string removedBreaks = texto.Replace("\r\n", replaceWith).Replace("\n", replaceWith).Replace("\r", replaceWith);
            return removedBreaks;
        }

        public static string TruncarTextoSiExcede(string texto, int maximo)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return texto;
            }

            return texto.Length <= maximo ? texto : texto.Substring(0, maximo);
        }


        public static string ConvertirBytesToBase64(byte[] bytes) { 
            return Convert.ToBase64String(bytes);
        }

    }
}
