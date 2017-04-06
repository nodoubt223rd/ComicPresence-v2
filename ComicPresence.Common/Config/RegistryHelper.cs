using System;

using Microsoft.Win32;

namespace ComicPresence.Common.Config
{
    public static class RegistryHelper
    {
        public static T GetValueOrDefault<T>(string valueName)
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\" + RegistryConfig.cApplicationKey))
            {
                if (key == null)
                    return default(T);

                object value = key.GetValue(valueName);

                if (value == null)
                    return default(T);

                return (T)Convert.ChangeType(value, typeof(T));
            }
        }
    }
}
