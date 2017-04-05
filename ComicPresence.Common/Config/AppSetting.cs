using System;

namespace ComicPresence.Common.Config
{
    public class AppSetting
    {
        public AppSettingId Id { get; set; }
        public string Value { get; set; }

        /// <summary>
        /// Must be a type supported by Convert.ChangeType
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetValue<T>()
        {
            if (typeof(T).IsEnum)
                return (T)Enum.Parse(typeof(T), Value);

            return (T)Convert.ChangeType(Value, typeof(T));
        }
    }
}
