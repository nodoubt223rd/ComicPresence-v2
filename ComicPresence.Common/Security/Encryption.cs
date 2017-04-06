using System;
using System.IO;
using System.Security.Cryptography;

namespace ComicPresence.Common.Security
{
    public static class Encryption
    {
        private const int cIvLengthBytes = 16;
        public const int cKeyLengthBytes = 32;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="keyBase64"></param>
        /// <returns>A base64-encoded encrypted string that includes cipher text and IV</returns>
        public static String EncryptString_Aes(string plainText, string keyBase64)
        {
            byte[] iv;
            byte[] encrypted = EncryptStringToBytes_Aes(plainText, keyBase64, out iv);
            byte[] combined = new byte[encrypted.Length + iv.Length];
            Array.Copy(encrypted, combined, encrypted.Length);
            Array.Copy(iv, 0, combined, encrypted.Length, iv.Length);

            return Convert.ToBase64String(combined);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptedString">A string encrypted using EncryptString_Aes</param>
        /// <param name="keyBase64"></param>
        /// <returns></returns>
        public static String DecryptString_Aes(string encryptedString, string keyBase64)
        {
            byte[] combined = Convert.FromBase64String(encryptedString);
            byte[] iv = new byte[cIvLengthBytes];
            byte[] encrypted = new byte[combined.Length - cIvLengthBytes];

            Array.Copy(combined, encrypted, combined.Length - cIvLengthBytes);
            Array.Copy(combined, encrypted.Length, iv, 0, cIvLengthBytes);
            return DecryptStringFromBytes_Aes(encrypted, keyBase64, iv);
        }

        /// <summary>
        /// AES-256 (256-bit key, 128-bit block size, 16-byte (128-bit) IV)
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="keyBase64">AES key, base64-encoded</param>
        /// <param name="iv">The new, randomly generated IV</param>
        /// <returns></returns>
        public static byte[] EncryptStringToBytes_Aes(string plainText, string keyBase64, out byte[] iv)
        {
            // Check arguments
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (keyBase64 == null || keyBase64.Length <= 0)
                throw new ArgumentNullException("keyBase64");

            byte[] key = Convert.FromBase64String(keyBase64);

            byte[] encrypted;

            // Create an Aes object with the specified key and IV
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                iv = aesAlg.IV;

                // Create a decryptor to perform the stream transform
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream
            return encrypted;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <param name="keyBase64">AES key, base64-encoded</param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string DecryptStringFromBytes_Aes(byte[] encryptedText, string keyBase64, byte[] iv)
        {
            // Check arguments
            if (encryptedText == null || encryptedText.Length <= 0)
                throw new ArgumentNullException("encryptedText");
            if (keyBase64 == null || keyBase64.Length <= 0)
                throw new ArgumentNullException("keyBase64");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");

            byte[] key = Convert.FromBase64String(keyBase64);

            // Declare the string used to hold the decrypted text
            string plaintext = null;

            // Create an Aes object with the specified key and IV
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                // Create a decryptor to perform the stream transform
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption
                using (MemoryStream msDecrypt = new MemoryStream(encryptedText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}
