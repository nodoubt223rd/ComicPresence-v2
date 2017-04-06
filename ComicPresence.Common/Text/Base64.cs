using System;
using System.Text;

namespace ComicPresence.Common.Text
{
    public class Base64
    {
        public static string EncodeUtf8(string rawStr)
        {
            byte[] strUtf8 = Encoding.UTF8.GetBytes(rawStr);
            return Convert.ToBase64String(strUtf8);
        }

        public static string DecodeUtf8(string base64Str)
        {
            byte[] strUtf8 = Convert.FromBase64String(base64Str);
            return Encoding.UTF8.GetString(strUtf8);
        }
    }
}
