using System;
using System.Configuration;

namespace ComicPresence.Common.Config
{
    public abstract class ConfigSettingsBase
    {
        public static T GetValue<T>(string key)
        {
            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));
        }
    }
}
