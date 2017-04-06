using System;
using System.Globalization;

namespace ComicPresence.Common.Utils
{
    public static class DateTimeExtensions
    {
        public static long ToUnixSeconds(this DateTime dt)
        {
            DateTimeOffset offset = dt;
            return offset.ToUnixTimeSeconds();
        }

        public static string ConvertMonthIntToString(int month)
        {
            DateTimeFormatInfo mfi = new DateTimeFormatInfo();

            return mfi.GetMonthName(month).ToString();
        }
    }
}
