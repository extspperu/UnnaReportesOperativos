using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios
{
    public static class Md5Utilitario
    {
        public static string? Cifrar(string? texto, string? salt)
        {
            if (texto == null || salt == null)
            {
                return null;
            }

            var saltBytes = Encoding.UTF8.GetBytes(salt);
            var textoBytes = Encoding.UTF8.GetBytes(texto);

            var hmacMD5 = new HMACMD5(saltBytes);
            var saltedHash = hmacMD5.ComputeHash(textoBytes);
            return Convert.ToBase64String(saltedHash);
        }

    }
}
