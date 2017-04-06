using System;
using log4net;
using log4net.Repository.Hierarchy;
using log4net.Appender;
using log4net.Layout;

using ComicPresence.Common.Config;
using ComicPresence.Common.Patterns;

namespace ComicPresence.Common.Logging
{
    /// <summary>
    /// Currently logs to an XML file.
    /// </summary>
    public class LogManager : Singleton<LogManager>
    {
        protected ILog InternalLog { get; set; }

        protected bool Configured { get; set; }

        private LoggingLevel CurrentLevel { get; set; }

        private LogManager()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="filePath"></param>
        /// <param name="minLoggingLevel"></param>
        /// <param name="maxFileSize">E.g. "1MB".</param>
        /// <param name="maxRollingBackupFiles"></param>
        public void Configure(string applicationName, string filePath, string maxFileSize = "1MB",
            int maxRollingBackupFiles = 60)
        {
            if (applicationName == null)
                throw new ArgumentNullException("applicationName");
            if (filePath == null)
                throw new ArgumentNullException("filePath");
            if (Configured)
                throw new InvalidOperationException("Already configured");

            Hierarchy hierarchy = (Hierarchy)log4net.LogManager.GetRepository();

            LoggingLevel level;
            string logLevelStr;

            using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Disburze"))
            {
                logLevelStr = (string)key.GetValue("LoggingLevel");
            }
            if (logLevelStr != null)
            {
                level = (LoggingLevel)Enum.Parse(typeof(LoggingLevel), logLevelStr);
            }
            else
            {
                level = ConfigSettings.LogLevel;
            }

            RollingFileAppender roller = new RollingFileAppender();
            roller.File = filePath;
            roller.DatePattern = ".yyyy-MM-dd-tt'.xml'";
            roller.AppendToFile = true;
            roller.RollingStyle = RollingFileAppender.RollingMode.Date;
            roller.StaticLogFileName = false;
            roller.MaxSizeRollBackups = maxRollingBackupFiles;
            roller.MaximumFileSize = maxFileSize;
            roller.Layout = new XmlLayoutSchemaLog4j();
            roller.Threshold = ConvertToLog4NetLevel(level);
            roller.ActivateOptions();

            hierarchy.Root.AddAppender(roller);

            hierarchy.Root.Level = ConvertToLog4NetLevel(level);
            hierarchy.RaiseConfigurationChanged(EventArgs.Empty);

            CurrentLevel = level;
            hierarchy.Configured = true;

            InternalLog = log4net.LogManager.GetLogger(applicationName);

            Configured = true;
        }

        /// <summary>
        /// Overload
        /// </summary>
        /// <param name="message"></param>
        public void LogError(string message)
        {
            CheckConfigured();

            Log(message, LoggingLevel.Error);
        }

        public void Log(string message, LoggingLevel level)
        {
            CheckConfigured();

            Log(message, null, level);
        }

        public void Log(string message, Exception ex, LoggingLevel level)
        {
            CheckConfigured();

            string serviceUsername = RequestLoggingHelper.GetCurrentServiceUsername();

            if (message == null)
                message = String.Empty;

            message = String.Format("{0}\r\n|{1}|", message, serviceUsername);

            InternalLog.Logger.Log(typeof(LogManager), ConvertToLog4NetLevel(level), message, ex);
        }

        public void LogError(string message, Exception ex)
        {
            Log(message, ex, LoggingLevel.Error);
        }

        /// <summary>
        /// Determines whether an entry logged at <paramref name="level"/> will be logged given current settings. 
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public bool WillLogLevel(LoggingLevel level)
        {
            CheckConfigured();

            return InternalLog.Logger.IsEnabledFor(ConvertToLog4NetLevel(level));
        }

        private void SetLoggingLevel(LoggingLevel minLoggingLevel)
        {
            if (CurrentLevel != minLoggingLevel)
            {
                Hierarchy hierarchy = (Hierarchy)log4net.LogManager.GetRepository();
                hierarchy.Root.Level = ConvertToLog4NetLevel(minLoggingLevel);
                CurrentLevel = minLoggingLevel;
            }
        }

        protected void CheckConfigured()
        {
            if (!Configured)
            {
                const string cMessage = "Error: LogManager instance is not configured";
                EventLogHelper.LogEvent(cMessage);

                throw new InvalidOperationException("Instance has not been configured");
            }
        }

        private log4net.Core.Level ConvertToLog4NetLevel(LoggingLevel level)
        {
            switch (level)
            {
                case LoggingLevel.Debug:
                    return log4net.Core.Level.Debug;
                case LoggingLevel.Info:
                    return log4net.Core.Level.Info;
                case LoggingLevel.Warn:
                    return log4net.Core.Level.Warn;
                case LoggingLevel.Error:
                    return log4net.Core.Level.Error;
                case LoggingLevel.Fatal:
                    return log4net.Core.Level.Fatal;
                default:
                    throw new NotImplementedException(String.Format("Unsupported LoggingLevel {0}", level));
            }
        }
    }
}
