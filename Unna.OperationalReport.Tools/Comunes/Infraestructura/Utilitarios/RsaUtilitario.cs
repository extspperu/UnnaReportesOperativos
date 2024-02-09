using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Web;

namespace Unna.OperationalReport.Tools.Comunes.Infraestructura.Utilitarios
{
    public static class RsaUtilitario
    {
        public static string? EncryptText(string? publicKey, string? text)
        {
            if (publicKey == null || text == null)
            {
                return null;
            }

            // Convert the text to an array of bytes   
            UnicodeEncoding byteConverter = new UnicodeEncoding();
            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(text);

            // Create a byte array to store the encrypted data in it   
            byte[] encryptedData;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024))
            {
                // Set the rsa pulic key   
                rsa.FromXmlString(publicKey);

                // Encrypt the data and store it in the encyptedData Array   
                encryptedData = rsa.Encrypt(dataToEncrypt, true);
                rsa.PersistKeyInCsp = false;
            }


            return Convert.ToBase64String(encryptedData);

        }

        public static string DecryptText(string privateKey, string encryptText)
        {
            // read the encrypted bytes from the file   
            byte[] dataToDecrypt = Convert.FromBase64String(encryptText);

            // Create an array to store the decrypted data in it   
            byte[] decryptedData;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024))
            {
                // Set the private key of the algorithm   
                rsa.FromXmlString(privateKey);
                decryptedData = rsa.Decrypt(dataToDecrypt, true);
                rsa.PersistKeyInCsp = false;
            }

            var decryptedTextData = Encoding.UTF8.GetString(decryptedData);
            return decryptedTextData.ToString();

        }


        public static T DescifrarId<T>(string cifrado, string llavePrivada)
        {
            return (T)Convert.ChangeType(DecryptText(llavePrivada, cifrado), typeof(T));
        }

        public static string? CifrarId<T>(T id, string llavePublica, bool crearParaUrl = false)
        {
            var objetoConvertidoATexto = Convert.ToString(id);
            if (objetoConvertidoATexto == null)
            {
                return null;
            }

            var cadenaCifrado = EncryptText(llavePublica, objetoConvertidoATexto);

            if (crearParaUrl)
            {
                return HttpUtility.UrlEncode(cadenaCifrado);
            }

            return cadenaCifrado;
        }
    }
}
