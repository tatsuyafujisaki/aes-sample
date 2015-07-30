using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SymmetricKeyAlgorithms
{
    class Program
    {
        static string Encrypt(SymmetricAlgorithm aes, string s)
        {
            var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write))
            {
                var bytes = Encoding.ASCII.GetBytes(s);
                cs.Write(bytes, 0, bytes.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        static string Decrypt(SymmetricAlgorithm aes, string encrypted)
        {
            var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateDecryptor(aes.Key, aes.IV), CryptoStreamMode.Write))
            {
                var bytes = Convert.FromBase64String(encrypted);
                cs.Write(bytes, 0, bytes.Length);
                cs.FlushFinalBlock();
                return Encoding.ASCII.GetString(ms.ToArray());
            }
        }

        static void Main()
        {
            const string original = "Here comes a string to encrypt.";
            var encrypted = "";
            var decrypted = "";

            // AesCryptoServiceProvider below can be replaced with ...
            // RijndaelManaged
            // DESCryptoServiceProvider
            // RC2CryptoServiceProvider
            // TripleDESCryptoServiceProvider
            // https://msdn.microsoft.com/en-us/library/system.security.cryptography.symmetricalgorithm.aspx

            using (var aes = new TripleDESCryptoServiceProvider())
            {
                encrypted = Encrypt(aes, original);
                decrypted = Decrypt(aes, encrypted);
            }

            Console.WriteLine("Encrypted: {0}", encrypted);
            Console.WriteLine("Decrypted: {0}", decrypted);
        }
    }
}