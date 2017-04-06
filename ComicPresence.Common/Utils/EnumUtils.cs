using System;

namespace ComicPresence.Common.Utils
{
    public static class EnumUtils
    {
        public static T ConvertEnum<T, TOther>(TOther value)
        {
            return (T)Enum.Parse(typeof(T), value.ToString());
        }

        public static TNew ConvertEnum<TNew>(object value)
        {
            return (TNew)Enum.Parse(typeof(TNew), value.ToString());
        }
    }
}
