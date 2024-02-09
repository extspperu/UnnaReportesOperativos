using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios
{
    public static class RijndaelUtilitario
    {
        #region Consts
        /// <summary>
        /// Change the Inputkey GUID when you use this code in your own program.
        /// Keep this inputkey very safe and prevent someone from decoding it some way!!
        /// </summary>
        internal const string Inputkey = "b8f9b344-8079-4e51-8334-c902c281d45";
        internal const string LLAVE_DEFECTO = "257046cc-3335-4eb9-9c9b-b12119e83449";
        #endregion

        public static string EncryptRijndael(string text)
        {
            return EncryptRijndael(text, LLAVE_DEFECTO);
        }

        public static string? EncryptRijndaelToUrl<T>(T objeto)
        {
            return EncryptRijndaelToUrl(objeto, LLAVE_DEFECTO);
        }


        public static string? EncryptRijndaelToUrl<T>(T objeto, string llave)
        {
            var cadenaDeObjeto = Convert.ToString(objeto);

            if (cadenaDeObjeto == null)
            {
                return null;
            }

            var plainTextBytes = Encoding.UTF8.GetBytes(EncryptRijndael(cadenaDeObjeto, llave));
            return Convert.ToBase64String(plainTextBytes);
        }


        public static T? DecryptRijndaelFromUrl<T>(string? cadena)
        {
            return DecryptRijndaelFromUrl<T>(cadena, LLAVE_DEFECTO);
        }


        public static T? DecryptRijndaelFromUrl<T>(string? cadena, string llave)
        {
            var resultado = default(T);
            if (cadena == null)
            {
                return resultado;
            }

            try
            {
                var base64EncodedBytes = Convert.FromBase64String(cadena);
                var cifrado = Encoding.UTF8.GetString(base64EncodedBytes);
                resultado = DecryptRijndael<T>(cifrado, llave);
            }
            catch { }

            return resultado;
        }

        public static T? DecryptRijndael<T>(string cifrado, string llave)
        {
            var resultado = default(T);

            try
            {
                resultado = (T)Convert.ChangeType(DecryptRijndael(cifrado, llave), typeof(T));
            }
            catch { }

            return resultado;
        }


        public static T? DecryptRijndael<T>(string cifrado)
        {
            return DecryptRijndael<T>(cifrado, LLAVE_DEFECTO);

        }

        #region Rijndael Encryption

        /// <summary>
        /// Encrypt the given text and give the byte array back as a BASE64 string
        /// </summary>
        /// <param name="text" />The text to encrypt
        /// <param name="salt" />The pasword salt
        /// <returns>The encrypted text</returns>
        public static string EncryptRijndael(string text, string salt)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("text");

            var aesAlg = NewRijndaelManaged(salt);

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(text);
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }
        #endregion

        #region Rijndael Dycryption
        /// <summary>
        /// Checks if a string is base64 encoded
        /// </summary>
        /// <param name="base64String" />The base64 encoded string
        /// <returns>Base64 encoded stringt</returns>
        public static bool IsBase64String(string base64String)
        {
            base64String = base64String.Trim();
            return (base64String.Length % 4 == 0) &&
                   Regex.IsMatch(base64String, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

        }

        /// <summary>
        /// Decrypts the given text
        /// </summary>
        /// <param name="cipherText" />The encrypted BASE64 text
        /// <param name="salt" />The pasword salt
        /// <returns>The decrypted text</returns>
        public static string DecryptRijndael(string cipherText, string salt)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");

            if (!IsBase64String(cipherText))
                throw new Exception("The cipherText input parameter is not base64 encoded");

            string text;

            var aesAlg = NewRijndaelManaged(salt);
            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            var cipher = Convert.FromBase64String(cipherText);

            using (var msDecrypt = new MemoryStream(cipher))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        text = srDecrypt.ReadToEnd();
                    }
                }
            }
            return text;
        }
        #endregion

        #region NewRijndaelManaged
        /// <summary>
        /// Create a new RijndaelManaged class and initialize it
        /// </summary>
        /// <param name="salt" />The pasword salt
        /// <returns></returns>
#pragma warning disable SYSLIB0022 // El tipo o el miembro están obsoletos
        private static RijndaelManaged NewRijndaelManaged(string salt)
#pragma warning restore SYSLIB0022 // El tipo o el miembro están obsoletos
        {
            if (salt == null) throw new ArgumentNullException("salt");
            var saltBytes = Encoding.ASCII.GetBytes(salt);
            var key = new Rfc2898DeriveBytes(Inputkey, saltBytes);

#pragma warning disable SYSLIB0022 // El tipo o el miembro están obsoletos
            var aesAlg = new RijndaelManaged();
#pragma warning restore SYSLIB0022 // El tipo o el miembro están obsoletos
            aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
            aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);

            return aesAlg;
        }
        #endregion
    }
}
