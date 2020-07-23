using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PJ_SICOI.LogicaNegocio.Clases
{
    public class Encriptacion
    {
        static byte[] keyAES = Convert.FromBase64String("FKe9d9hoqr40co1RrJIvP7dGLvtArSpOoP0UmrApZtU=");
        static byte[] IVAES = Convert.FromBase64String("FKe9d9hoqr40co1RrJIvP7dGLvtArSpOoP0UmrApZtU=");
        public static string EncriptarMD5(string contrasena)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] BytesEntrada = Encoding.ASCII.GetBytes(contrasena);
                byte[] HashBytes = md5.ComputeHash(BytesEntrada);

                // Convierte el arreglo de bytes en un string hexadecimal
                StringBuilder V_ContrasenaEncriptada = new StringBuilder();
                for (int i = 0; i < HashBytes.Length; i++)
                {
                    V_ContrasenaEncriptada.Append(HashBytes[i].ToString("X2"));
                }
                return V_ContrasenaEncriptada.ToString().ToLower();
            }
        }

        public static string EncryptStringToBytes_Aes(string plainText)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyAES;
                aesAlg.IV = IVAES;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return Encoding.ASCII.GetString(encrypted);
        }
    }
}
