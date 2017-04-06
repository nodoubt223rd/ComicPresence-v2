using ComicPresence.Common.Logging;

namespace ComicPresence.Common.Config
{
    /// <summary>
    /// Common/shared web/app.config settings. Application/component-specific settings should be implemented elsewhere. 
    /// Most settings, including those that can change at run-time. 
    /// </summary>
    public class ConfigSettings : ConfigSettingsBase
    {
        private ConfigSettings()
        {
        }

        public static string LogFilePath
        {
            get
            {
                return GetValue<string>(ConfigSettingKey.LogFilePath);
            }
        }

        public static string PerformanceLogPath
        {
            get
            {
                return GetValue<string>(ConfigSettingKey.PerformanceLogPath);
            }
        }

        public static string ApplicationVersion
        {
            get
            {
                return GetValue<string>(ConfigSettingKey.ApplicationVersion);
            }
        }

        public static string ClusterAuthToken
        {
            get
            {
                return GetValue<string>(ConfigSettingKey.ClusterAuthToken);
            }
        }

        public static string DefaultNamespace
        {
            get
            {
                return GetValue<string>(ConfigSettingKey.DefaultNamespace);
            }
        }

        public static bool EnableCaching
        {
            get
            {
                return GetValue<bool>(ConfigSettingKey.EnableCaching);
            }
        }       

        public static LoggingLevel LogLevel
        {
            get
            {
                return GetValue<LoggingLevel>(ConfigSettingKey.LogLevel);
            }
        }

        public static string RegLinkDomain
        {
            get
            {
                return GetValue<string>(ConfigSettingKey.RegLinkDomain);
            }
        }

        public static string ServiceUserName
        {
            get
            {
                return GetValue<string>(ConfigSettingKey.ServiceUserName);
            }
        }

        public static string ServicePassword
        {
            get
            {
                return GetValue<string>(ConfigSettingKey.ServicePassword);
            }
        }

        public static string PrimaryConnStringName
        {
            get
            {
                return GetValue<string>(ConfigSettingKey.PrimaryConnStringName);
            }
        }

        public static string SecondaryConnStringName
        {
            get
            {
                return GetValue<string>(ConfigSettingKey.SecondaryConnStringName);
            }
        }

        public static string AppConnStringName
        {
            get
            {
                return GetValue<string>(ConfigSettingKey.AppConnStringName);
            }
        }

        public static string CoreConnStringName
        {
            get
            {
                return GetValue<string>(ConfigSettingKey.CoreConnStringName);
            }
        }
    }
}
