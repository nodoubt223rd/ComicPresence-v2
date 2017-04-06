using System;
using System.Text;
using System.Security.Cryptography;

namespace ComicPresence.Common.Security
{
    public static class MD5Hash
    {
        /// <summary>
        /// Matches SQL Server HashBytes('MD5', varchar(n))
        /// Returns a hex string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMd5HashHexStr(string input)
        {
            MD5 md5Hash = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.ASCII.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// Matches SQL Server HashBytes('MD5', varchar(n)) 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMd5HashBase64(string input)
        {
            return Convert.ToBase64String(GetMd5HashBytes(input));
        }

        /// <summary>
        /// Matches SQL Server HashBytes('MD5', varchar(n))
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] GetMd5HashBytes(string input)
        {
            MD5 md5Hash = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.ASCII.GetBytes(input));

            return data;
        }
    }
}
