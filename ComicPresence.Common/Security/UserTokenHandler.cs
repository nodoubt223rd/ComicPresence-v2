using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace ComicPresence.Common.Security
{
    public static class UserTokenHandler
    {
        /// <summary>
        /// Includes a UTC date/time in the token.
        /// </summary>
        /// <param name="id">E.g. a user ID</param>
        /// <param name="type">Arbitrary type string, e.g. "PasswordReset"</param>
        /// <returns>The token</returns>
        public static string GeneratePasswordResetToken(string id, string type)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream, new UTF8Encoding(false, true), true))
            {
                binaryWriter.Write(DateTime.UtcNow.Ticks);
                binaryWriter.Write(Convert.ToString(id, CultureInfo.InvariantCulture));
                binaryWriter.Write("PasswordReset");
            }
            byte[] tokenBytes = System.Web.Security.MachineKey.Protect(memoryStream.ToArray());
            string tokenStr = Convert.ToBase64String(tokenBytes);
            return tokenStr;
        }

        /// <summary>
        /// Includes a UTC date/time in the token.
        /// </summary>
        /// <param name="id">E.g. a user ID</param>
        /// <param name="type">Arbitrary type string, e.g. "PasswordReset"</param>
        /// <returns>The token</returns>
        public static string GenerateValidateEmailToken(string id, string type)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream, new UTF8Encoding(false, true), true))
            {
                binaryWriter.Write(DateTime.UtcNow.Ticks);
                binaryWriter.Write(Convert.ToString(id, CultureInfo.InvariantCulture));
                binaryWriter.Write("ValidateEmail");
            }
            byte[] tokenBytes = System.Web.Security.MachineKey.Protect(memoryStream.ToArray());
            string tokenStr = Convert.ToBase64String(tokenBytes);
            return tokenStr;
        }

        /// <summary>
        /// Validates a token. Does not validate data inside token, such as date/time. 
        /// </summary>
        /// <param name="token"></param>
        /// <returns>A UserToken if validated, null otherwise. </returns>
        public static UserToken ValidateToken(string token)
        {
            byte[] buffer = System.Web.Security.MachineKey.Unprotect(Convert.FromBase64String(token));
            MemoryStream memoryStream = new MemoryStream(buffer);
            UserToken userToken = new UserToken();
            using (BinaryReader binaryReader = new BinaryReader(memoryStream, new UTF8Encoding(false, true), true))
            {
                userToken.DateTimeUtc = new DateTime(binaryReader.ReadInt64(), DateTimeKind.Utc);
                userToken.Id = binaryReader.ReadString();
                userToken.Type = binaryReader.ReadString();
            }
            return userToken;
        }
    }
}
