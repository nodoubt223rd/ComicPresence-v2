
namespace ComicPresence.Common.Config
{
    /// <summary>
    /// Common web/app .config app setting keys. Application/component-specific keys should be implemented elsewhere. 
    /// Most settings, including those that can change at run-time, should be stored in the AppSetting table instead. 
    /// </summary>
    public class ConfigSettingKey
    {
        public const string LogFilePath = "LogFilePath";
        public const string LogLevel = "LoggingLevel";
        public const string PerformanceLogPath = "PerformanceLogPath";
        public const string ClusterAuthToken = "ClusterAuthToken";
        public const string ApplicationVersion = "ApplicationVersion";
        public const string EnableCaching = "EnableCaching";
        public const string DefaultNamespace = "DefaultNamespace";
        public const string RegLinkDomain = "RegLinkDomain";
        public const string ServiceUserName = "ServiceUserName";
        public const string ServicePassword = "ValueToken";
        public const string PrimaryConnStringName = "Primary";
        public const string SecondaryConnStringName = "Secondary";
        public const string AppConnStringName = "App";
        public const string CoreConnStringName = "Core";
    }
}
